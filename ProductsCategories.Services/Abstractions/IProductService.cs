using ProductsCategories.Services.Models.Product;

namespace ProductsCategories.Services.Abstractions;

public interface IProductService
{
    Task<ProductResponse?> GetProductByIdAsync(int id);
    Task<List<ProductResponse>?> GetAllProductsAsync(int pageNumber, int pageSize);
    Task<ProductResponse?> CreateProductAsync(ProductCreateRequest product);
    Task<ProductResponse?> UpdateProductNameAsync(int id, string productName);
    Task<ProductResponse?> UpdateProductCategoriesAsync(int id, params int[] categoryIds);
    Task<ProductResponse?> DeleteProductAsync(int id);
}
