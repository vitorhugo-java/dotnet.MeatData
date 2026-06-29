using MeatData.Application.Common;
using MeatData.Application.Interfaces.Repositories;
using MeatData.Application.Products.DTOs;
using MeatData.Domain.Entities;

namespace MeatData.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdHandler
{
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(IProductRepository repository)
        => _repository = repository;

    public async Task<Result<ProductDto>> Handle(
        GetProductByIdQuery query, CancellationToken ct = default)
    {
        var product = await _repository.GetByIdAsync(query.Id, ct);

        if (product is null)
            return Result<ProductDto>.Failure($"Produto '{query.Id}' não encontrado.", "PRODUCT_NOT_FOUND");

        return Result<ProductDto>.Success(MapToDto(product));
    }

    // Método estático de mapeamento — sem AutoMapper, sem dependência extra
    // No Spring Boot você usaria um ModelMapper ou MapStruct aqui
    internal static ProductDto MapToDto(Product p) => new(
        p.Id,
        p.Name,
        p.Description,
        p.SKU,
        p.WeightGrams,
        new CategoryDto(p.Category.Id, p.Category.Name, p.Category.Description, p.Category.animalCategory),
        p.NutritionProfile is null ? null : new NutritionProfileDto(
            p.NutritionProfile.Id,
            p.NutritionProfile.FdclId,
            p.NutritionProfile.Calories,
            p.NutritionProfile.ProteinGrams,
            p.NutritionProfile.FatGrams,
            p.NutritionProfile.CarbsGrams,
            p.NutritionProfile.SodiumMg,
            p.NutritionProfile.FetchedAt,
            p.NutritionProfile.Source),
        p.CreatedAt,
        p.UpdatedAt);
}
