using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsCategories.Data;
using ProductsCategories.Data.Entities;
using ProductsCategories.Services.Abstractions;
using ProductsCategories.Services.Extensions;
using ProductsCategories.Services.Models.Category;

namespace ProductsCategories.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<CategoryService> _logger;
    public CategoryService(AppDbContext dbContext, ILogger<CategoryService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<CategoryResponse?> CreateCategoryAsync(CategoryCreateRequest category)
    {
        _logger.LogInformation("Creating category with name: {Name}", category.Name);

        var newCategory = new Category { Name = category.Name };

        try
        {
            _dbContext.Categories.Add(newCategory);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Category created successfully with ID: {Id}", newCategory.Id);

            var response = new CategoryResponse(newCategory.Id, newCategory.Name);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category with name: {Name}", category.Name);
            return null;
        }
    }

    public async Task<CategoryResponse?> DeleteCategoryAsync(int id)
    {
        _logger.LogInformation("Deleting category with ID: {Id}", id);

        var category = await _dbContext.Categories.FindAsync(id);
        if (category == null)
        {
            _logger.LogWarning("Category with ID: {Id} not found", id);
            return null;
        }

        try
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Category with ID: {Id} deleted successfully", id);

            var response = new CategoryResponse(category.Id, category.Name);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category with ID: {Id}", id);
            return new CategoryResponse(0, "");
        }
    }

    public async Task<List<CategoryResponse>> GetAllCategoriesAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation("Retrieving paginated categories: page {Page}, size {Size}", pageNumber, pageSize);

        try
        {
            var response = await _dbContext.Categories
                .Select(c => new CategoryResponse(
                    c.Id,
                    c.Name
                ))
                .ToPaginatedListAsync(pageNumber, pageSize);

            _logger.LogInformation("Retrieved {Count} categories", response.Count);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving categories");
            return new List<CategoryResponse>();
        }
    }

    public async Task<CategoryResponse?> GetCategoryByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving category with ID: {Id}", id);

        var category = await _dbContext.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryResponse(
                c.Id,
                c.Name
            )).FirstOrDefaultAsync();

        if (category == null)
        {
            _logger.LogWarning("Category with ID: {Id} not found", id);
            return null;
        }

        _logger.LogInformation("Retrieved category with ID: {Id}", id);
        return category;
    }

    public async Task<CategoryResponse?> UpdateCategoryAsync(int id, CategoryCreateRequest category)
    {
        _logger.LogInformation("Updating category with name: {Name}", category.Name);

        var existingCategory = _dbContext.Categories.Find(id);
        if (existingCategory == null)
        {
            _logger.LogWarning("Category with ID: {Id} not found", id);
            return null;
        }

        try
        {
            existingCategory.Name = category.Name;
            _dbContext.Categories.Update(existingCategory);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Category with ID: {Id} updated successfully", existingCategory.Id);
            var response = new CategoryResponse(existingCategory.Id, existingCategory.Name);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category with ID: {Id}", id);
            return new CategoryResponse(0, "");
        }
    }
}
