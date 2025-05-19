using Microsoft.AspNetCore.Mvc;
using ProductsCategories.Services.Abstractions;
using ProductsCategories.Services.Models.Product;

namespace ProductsCategories.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            return BadRequest($"Product with ID {id} not found");
        }

        if (product.Id == 0)
        {
            return BadRequest("Something went wrong");
        }

        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var products = await _productService.GetAllProductsAsync(pageNumber, pageSize);

        if (products == null)
        {
            return BadRequest("Something went wrong");
        }

        return Ok(products);
    }


    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdProduct = await _productService.CreateProductAsync(product);

        if (createdProduct == null)
        {
            return BadRequest("Product should have 2 or 3 categories");
        }

        if (createdProduct.Id == 0)
        {
            return BadRequest("Something went wrong");
        }

        return Ok(createdProduct);
    }

    [HttpPatch("{id}/Categories")]
    public async Task<IActionResult> UpdateCategories(int id, [FromBody] int[] categoryIds)
    {
        var result = await _productService.UpdateProductCategoriesAsync(id, categoryIds);

        if (result == null)
            return BadRequest($"Product with ID {id} not found.");

        if (result.Id == 0)
            return BadRequest("Something went wrong");

        return Ok(result);
    }

    [HttpPatch("{id}/Name")]
    public async Task<IActionResult> UpdateProductName(int id, [FromBody] string productName)
    {
        var result = await _productService.UpdateProductNameAsync(id, productName);

        if (result == null)
            return BadRequest($"Product with ID {id} not found.");

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _productService.DeleteProductAsync(id);

        if (product == null)
        {
            return BadRequest($"Product with ID {id} not found");
        }

        if (product.Id == 0)
        {
            return BadRequest("Something went wrong");
        }

        return Ok(product);
    }
}