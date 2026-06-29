using MeatData.Application.Products.DTOs;
using MeatData.Application.Common;

namespace MeatData.Application.Products.Queries.GetProducts;

// No Spring Boot isso seria um parâmetro do @GetMapping — aqui vira um objeto de query explícito
public record GetProductsQuery(Guid? CategoryId = null);
