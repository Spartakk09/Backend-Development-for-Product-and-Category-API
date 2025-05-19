using ProductsCategories.Services.Models.Category;

namespace ProductsCategories.Services.Abstractions;

public interface ICategoryService
{
    Task<CategoryResponse?> GetCategoryByIdAsync(int id);
    Task<List<CategoryResponse>> GetAllCategoriesAsync(int pageNumber, int pageSize);
    Task<CategoryResponse?> CreateCategoryAsync(CategoryCreateRequest category);
    Task<CategoryResponse?> UpdateCategoryAsync(int id, CategoryCreateRequest category);
    Task<CategoryResponse?> DeleteCategoryAsync(int id);
}
