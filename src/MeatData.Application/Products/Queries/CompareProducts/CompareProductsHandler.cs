using MeatData.Application.Common;
using MeatData.Application.Interfaces.Repositories;
using MeatData.Application.Products.Queries.GetProductById;

namespace MeatData.Application.Products.Queries.CompareProducts;

public sealed class CompareProductsHandler
{
    private readonly IProductRepository _repository;

    public CompareProductsHandler(IProductRepository repository)
        => _repository = repository;

    public async Task<Result<ComparisonDto>> Handle(
        CompareProductsQuery query, CancellationToken ct = default)
    {
        if (query.Ids.Count < 2)
            return Result<ComparisonDto>.Failure("Informe ao menos 2 produtos para comparar.", "INSUFFICIENT_PRODUCTS");

        // Busca em paralelo — no Spring Boot seria um CompletableFuture.allOf()
        var tasks = query.Ids.Select(id => _repository.GetByIdAsync(id, ct));
        var products = await Task.WhenAll(tasks);

        var notFound = query.Ids
            .Zip(products, (id, p) => (id, p))
            .Where(x => x.p is null)
            .Select(x => x.id)
            .ToList();

        if (notFound.Count > 0)
            return Result<ComparisonDto>.Failure(
                $"Produtos não encontrados: {string.Join(", ", notFound)}",
                "PRODUCT_NOT_FOUND");

        var dtos = products.Select(p => GetProductByIdHandler.MapToDto(p!)).ToList();
        return Result<ComparisonDto>.Success(new ComparisonDto(dtos));
    }
}
