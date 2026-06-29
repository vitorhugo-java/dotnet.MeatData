using MeatData.Application.Interfaces;
using MeatData.Application.Interfaces.ExternalApis;
using MeatData.Application.Interfaces.Repositories;
using MeatData.Application.Products.Commands.CreateProduct;
using MeatData.Application.Products.Queries.CompareProducts;
using MeatData.Application.Products.Queries.GetProductById;
using MeatData.Application.Products.Queries.GetProducts;
using MeatData.Infrastructure.Cache;
using MeatData.Infrastructure.ExternalApis.FoodDataCentral;
using MeatData.Infrastructure.Persistence;
using MeatData.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// ─── Controllers ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ─── Database ─────────────────────────────────────────────────────────────────
// Equivalente ao @EnableJpaRepositories + spring.datasource no application.yml
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// ─── Redis ────────────────────────────────────────────────────────────────────
// IDistributedCache — equivalente ao @EnableCaching + @Cacheable do Spring
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Redis"));

// ─── Repositories & Unit of Work ─────────────────────────────────────────────
// No Spring Boot os @Repository são detectados por scan — aqui é explícito
// Scoped = uma instância por request HTTP (equivalente ao @RequestScope)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ─── Application Handlers ────────────────────────────────────────────────────
// Sem MediatR por agora — injeção direta como @Service no Spring Boot
// Se o projeto crescer, vale adicionar MediatR para desacoplar os controllers dos handlers
builder.Services.AddScoped<CreateProductHandler>();
builder.Services.AddScoped<GetProductsHandler>();
builder.Services.AddScoped<GetProductByIdHandler>();
builder.Services.AddScoped<CompareProductsHandler>();

// ─── FoodData Central HttpClient + Resiliência ───────────────────────────────
builder.Services.AddOptions<FoodDataCentralOptions>()
    .BindConfiguration(FoodDataCentralOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Registra o client real como named (não como interface) para o decorator conseguir resolvê-lo
builder.Services.AddHttpClient<FoodDataCentralClient>((sp, client) =>
{
    var opts = sp.GetRequiredService<IOptions<FoodDataCentralOptions>>().Value;
    client.BaseAddress = new Uri(opts.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(15);
}).AddResilienceHandler("fdc-pipeline", pipeline =>
{
    pipeline.AddRetry(new HttpRetryStrategyOptions
    {
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromSeconds(1),
        BackoffType = DelayBackoffType.Exponential,
        ShouldHandle = args => ValueTask.FromResult(
            args.Outcome.Result?.StatusCode is HttpStatusCode.TooManyRequests
                or HttpStatusCode.ServiceUnavailable
                or HttpStatusCode.GatewayTimeout)
    });
    pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
    {
        FailureRatio = 0.5,
        MinimumThroughput = 5,
        BreakDuration = TimeSpan.FromSeconds(30)
    });
    pipeline.AddTimeout(TimeSpan.FromSeconds(10));
});

// Decorator Pattern: quem pede IFoodDataCentralClient recebe o Cached que chama o real por dentro
// Equivalente ao @Primary + @Qualifier no Spring Boot
builder.Services.AddScoped<IFoodDataCentralClient>(sp =>
    new CachedFoodDataCentralClient(
        sp.GetRequiredService<FoodDataCentralClient>(),
        sp.GetRequiredService<Microsoft.Extensions.Caching.Distributed.IDistributedCache>(),
        sp.GetRequiredService<IOptions<FoodDataCentralOptions>>()));

// ─── Pipeline ─────────────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Scalar UI em /scalar/v1 (substituto do Swagger UI no .NET 9)
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Expõe a classe Program para os testes de integração (WebApplicationFactory)
// No Spring Boot seria o @SpringBootApplication detectado automaticamente
public partial class Program { }
