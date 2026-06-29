namespace MeatData.Application.Products.DTOs;

public record NutritionProfileDto(
    Guid Id,
    string FdcId,
    decimal Calories,
    decimal ProteinGrams,
    decimal FatGrams,
    decimal CarbsGrams,
    decimal SodiumMg,
    DateTime FetchedAt,
    string Source);
