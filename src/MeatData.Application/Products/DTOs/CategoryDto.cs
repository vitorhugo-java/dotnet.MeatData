using MeatData.Domain.Enums;

namespace MeatData.Application.Products.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string Description,
    AnimalCategory AnimalCategory);
