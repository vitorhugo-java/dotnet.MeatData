using MeatData.Application.Products.Commands.CreateProduct;
using MeatData.Application.Products.Queries.CompareProducts;
using MeatData.Application.Products.Queries.GetProductById;
using MeatData.Application.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;

namespace MeatData.Api.Controllers;

// [ApiController] faz três coisas importantes automaticamente:
//   1. Valida ModelState e retorna 400 sem você pedir
//   2. Resolve parâmetros de rota/body automaticamente ([FromBody] implícito em POST/PUT)
//   3. Formata erros de validação como ProblemDetails (RFC 7807)
// Equivalente à combinação de @RestController + @Validated no Spring Boot

[ApiController]
[Route("api/[controller]")]  // → /api/products
public class ProductsController : ControllerBase
{
    private readonly CreateProductHandler _createHandler;
    private readonly GetProductsHandler _getProductsHandler;
    private readonly GetProductByIdHandler _getByIdHandler;
    private readonly CompareProductsHandler _compareHandler;

    public ProductsController(
        CreateProductHandler createHandler,
        GetProductsHandler getProductsHandler,
        GetProductByIdHandler getByIdHandler,
        CompareProductsHandler compareHandler)
    {
        _createHandler = createHandler;
        _getProductsHandler = getProductsHandler;
        _getByIdHandler = getByIdHandler;
        _compareHandler = compareHandler;
    }

    // GET /api/products
    // GET /api/products?categoryId=xxx
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? categoryId,
        CancellationToken ct)
    {
        var result = await _getProductsHandler.Handle(new GetProductsQuery(categoryId), ct);
        return Ok(result.Value);
    }

    // GET /api/products/{id}
    // [FromRoute] é implícito quando o nome do param bate com a rota — igual ao @PathVariable do Spring
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _getByIdHandler.Handle(new GetProductByIdQuery(id), ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { result.ErrorMessage });
    }

    // GET /api/products/compare?ids=xxx,yyy
    // Rota literal "compare" antes de {id:guid} para o ASP.NET Core não confundir as duas
    [HttpGet("compare")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Compare(
        [FromQuery] string ids,
        CancellationToken ct)
    {
        var parsedIds = ids.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => Guid.TryParse(s.Trim(), out var g) ? (Guid?)g : null)
            .ToList();

        if (parsedIds.Any(id => id is null))
            return BadRequest(new { ErrorMessage = "Um ou mais IDs são inválidos." });

        var query = new CompareProductsQuery(parsedIds.Select(g => g!.Value).ToList());
        var result = await _compareHandler.Handle(query, ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ErrorCode switch
            {
                "PRODUCT_NOT_FOUND" => NotFound(new { result.ErrorMessage }),
                _ => BadRequest(new { result.ErrorMessage })
            };
    }

    // POST /api/products
    // [FromBody] é implícito em POST com [ApiController] — igual ao @RequestBody do Spring
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductCommand command,
        CancellationToken ct)
    {
        var result = await _createHandler.Handle(command, ct);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value })
            : result.ErrorCode switch
            {
                "DUPLICATE_SKU" => Conflict(new { result.ErrorMessage }),
                _ => BadRequest(new { result.ErrorMessage })
            };
    }

    // DELETE /api/products/{id}
    // 204 No Content — sem body na resposta (convenção REST)
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _getByIdHandler.Handle(new GetProductByIdQuery(id), ct);

        if (result.IsFailure)
            return NotFound(new { result.ErrorMessage });

        // TODO: DeleteProductHandler — por ora retorna 204 se encontrado
        // (exercício: criar DeleteProductCommand + Handler seguindo o mesmo padrão do Create)
        return NoContent();
    }
}
