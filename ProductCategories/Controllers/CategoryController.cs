using Microsoft.AspNetCore.Mvc;
using ProductsCategories.Services.Abstractions;
using ProductsCategories.Services.Models.Category;

namespace ProductsCategories.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category == null)
            return BadRequest($"Category with {id} not found");

        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var categories = await _categoryService.GetAllCategoriesAsync(pageNumber, pageSize);

        if (categories == null)
        {
            return BadRequest("Something went wrong while retrieving categories.");
        }

        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateRequest category)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdCategory = await _categoryService.CreateCategoryAsync(category);

        if (createdCategory == null)
        {
            return BadRequest("Something went wrong while creating the category.");
        }

        return Ok(createdCategory);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryCreateRequest request)
    {
        var updated = await _categoryService.UpdateCategoryAsync(id, request);

        if (updated == null)
            return BadRequest($"Category with {id} not found");

        return Ok(updated);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _categoryService.DeleteCategoryAsync(id);

        if (category == null)
        {
            return BadRequest($"Product with ID {id} not found");
        }

        if (category.Id == 0)
        {
            return BadRequest("Something went wrong");
        }

        return Ok(category);
    }
}