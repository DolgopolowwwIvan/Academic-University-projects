using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceListManager.API.Models.DTOs;
using PriceListManager.API.Services;

namespace PriceListManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(
        [FromQuery] int? categoryId = null,
        [FromQuery] string? search = null)
    {
        var products = await _productService.GetAllAsync(categoryId, search);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Search([FromQuery] ProductSearchRequest request)
    {
        var products = await _productService.SearchAsync(request);
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "Название товара обязательно" });
        }

        if (request.Price <= 0)
        {
            return BadRequest(new { message = "Цена должна быть больше 0" });
        }

        if (request.Quantity < 0)
        {
            return BadRequest(new { message = "Количество не может быть отрицательным" });
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var (product, error) = await _productService.CreateAsync(request, userId);

        if (error != null)
        {
            return BadRequest(new { message = error });
        }

        return CreatedAtAction(nameof(GetById), new { id = product!.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { message = "Название товара обязательно" });
        }

        if (request.Price <= 0)
        {
            return BadRequest(new { message = "Цена должна быть больше 0" });
        }

        if (request.Quantity < 0)
        {
            return BadRequest(new { message = "Количество не может быть отрицательным" });
        }

        var (success, error) = await _productService.UpdateAsync(id, request);
        
        if (!success)
        {
            return BadRequest(new { message = error });
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/move")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<ProductDto>> Move(int id, [FromBody] MoveProductRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var (product, error) = await _productService.MoveAsync(id, request.TargetCategoryId, userId);

        if (error != null)
        {
            return BadRequest(new { message = error });
        }

        return Ok(product);
    }

    [HttpPost("move-bulk")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult> MoveBulk([FromBody] MoveBulkRequest request)
    {
        if (request.ProductIds == null || !request.ProductIds.Any())
        {
            return BadRequest(new { message = "Не выбраны товары для перемещения" });
        }

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var (movedCount, errors) = await _productService.MoveBulkAsync(request, userId);

        return Ok(new { movedCount, errors });
    }

    [HttpGet("{id}/movements")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<ActionResult<IEnumerable<ProductMovementDto>>> GetMovements(int id)
    {
        var movements = await _productService.GetMovementsAsync(id);
        return Ok(movements);
    }
}
