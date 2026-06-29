using MeatData.Application.Common;
using MeatData.Application.Interfaces.Repositories;
using MeatData.Application.Products.DTOs;
using MeatData.Domain.Entities;

namespace MeatData.Application.Products.Queries.GetProducts;

public sealed class GetProductsHandler
{
    private readonly IProductRepository _repository;

    public GetProductsHandler(IProductRepository repository)
        => _repository = repository;

    public async Task<Result<IReadOnlyList<ProductSummaryDto>>> Handle(
        GetProductsQuery query, CancellationToken ct = default)
    {
        var products = query.CategoryId.HasValue
            ? await _repository.GetByCategoryAsync(query.CategoryId.Value, ct)
            : await _repository.GetAllAsync(ct);

        var dtos = products.Select(MapToSummary).ToList();
        return Result<IReadOnlyList<ProductSummaryDto>>.Success(dtos);
    }

    private static ProductSummaryDto MapToSummary(Product p) => new(
        p.Id,
        p.Name,
        p.SKU,
        p.WeightGrams,
        p.Category?.Name ?? "—",
        p.NutritionProfile is not null,
        p.CreatedAt);
}
