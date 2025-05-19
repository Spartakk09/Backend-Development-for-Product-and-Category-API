using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsCategories.Data;
using ProductsCategories.Data.Entities;
using ProductsCategories.Services.Abstractions;
using ProductsCategories.Services.Extensions;
using ProductsCategories.Services.Models.Category;
using ProductsCategories.Services.Models.Product;

namespace ProductsCategories.Services.Implementations;

public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ProductService> _logger;
    public ProductService(AppDbContext dbContext, ILogger<ProductService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<ProductResponse?> CreateProductAsync(ProductCreateRequest product)
    {
        _logger.LogInformation("Creating product with name: {Name}", product.Name);

        if (product.CategoryIds.Count != 2 && product.CategoryIds.Count != 3)
        {
            _logger.LogWarning("Product don't have 2 or 3 categories");
            return null;
        }

        var newProduct = new Product { Name = product.Name };

        try
        {
            foreach (var categoryId in product.CategoryIds)
            {
                var category = await _dbContext.Categories.FindAsync(categoryId);
                if (category == null)
                {
                    _logger.LogError("Category with ID: {Id} not found", categoryId);
                    throw new Exception($"Category with ID: {categoryId} not found");
                }
                newProduct.ProductCategories.Add(new ProductCategory { Product = newProduct, Category = category });
            }

            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Product created successfully with ID: {Id}", newProduct.Id);

            var response = new ProductResponse(
                newProduct.Id,
                newProduct.Name,
                newProduct.ProductCategories
                .Select(pc => new CategoryResponse(pc.Category.Id, pc.Category.Name)).ToList()
            );

            return response;
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product with name: {Name}", product.Name);
            return new ProductResponse(0, "", []);
        }
    }

    public async Task<ProductResponse?> DeleteProductAsync(int id)
    {
        _logger.LogInformation("Deleting product with ID: {Id}", id);

        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID: {Id} not found", id);
            return null;
        }

        try
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Product with ID: {Id} deleted successfully", id);

            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.ProductCategories
                .Select(pc => new CategoryResponse(pc.Category.Id, pc.Category.Name)).ToList()
            );

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with ID: {Id}", id);
            return new ProductResponse(0, "", []);
        }
    }

    public async Task<List<ProductResponse>?> GetAllProductsAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation("Fetching paginated products");

        try
        {
            var products = await _dbContext.Products
                .Select(p => new ProductResponse(
                    p.Id,
                    p.Name,
                    p.ProductCategories
                        .Select(pc => new CategoryResponse(pc.Category.Id, pc.Category.Name))
                        .ToList()
                ))
                .ToPaginatedListAsync(pageNumber, pageSize); 

            _logger.LogInformation("Fetched {Count} paginated products", products.Count);
            return products;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching paginated products");
            return null;
        }
    }


    public async Task<ProductResponse?> GetProductByIdAsync(int id)
    {
        _logger.LogInformation("Fetching product with ID: {Id}", id);

        try
        {
            var product = await _dbContext.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductResponse(
                    p.Id,
                    p.Name,
                    p.ProductCategories
                    .Where(pc => pc.ProductId == p.Id)
                    .Select(pc => new CategoryResponse(pc.Category.Id, pc.Category.Name)).ToList())
                ).FirstOrDefaultAsync();

            if (product == null)
            {
                _logger.LogWarning("Product with ID: {Id} not found", id);
                return null;
            }

            _logger.LogInformation("Fetched product with ID: {Id}", id);
            return product;
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product with ID: {Id}", id);
            return new ProductResponse(0, "", []);
        }


    }

    public async Task<ProductResponse?> UpdateProductCategoriesAsync(int id, params int[] categoryIds)
    {
        _logger.LogInformation("Updating categories for product with ID: {Id}", id);

        var product = await _dbContext.Products
            .Include(p => p.ProductCategories)
            .ThenInclude(pc => pc.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID: {Id} not found", id);
            return null;
        }

        if (categoryIds.Length != 2 && categoryIds.Length != 3)
        {
            _logger.LogWarning(
                "Invalid number of categories provided for product ID {Id}. Expected 2 or 3, got: {Count}",
                id,
                categoryIds.Length
            );
            return new ProductResponse(0, "Product must have 2 or 3 categories", []);
        }

        // Get all valid categories from DB
        var categories = await _dbContext.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync();

        if (categories.Count != categoryIds.Length)
        {
            var missingIds = categoryIds.Except(categories.Select(c => c.Id));
            var message = $"Invalid category";

            _logger.LogWarning("Update failed. {Message}", message);
            return new ProductResponse(0, message, []);
        }

        try
        {
            // Clear current categories
            product.ProductCategories.Clear();

            // Add new valid categories
            foreach (var category in categories)
            {
                product.ProductCategories.Add(new ProductCategory
                {
                    Product = product,
                    Category = category
                });
            }

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Updated categories for product with ID: {Id}", id);

            return new ProductResponse(
                product.Id,
                product.Name,
                product.ProductCategories.Select(pc =>
                    new CategoryResponse(pc.Category.Id, pc.Category.Name)).ToList()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating categories for product with ID: {Id}", id);
            return null;
        }
    }


    public async Task<ProductResponse?> UpdateProductNameAsync(int id, string productName)
    {
        _logger.LogInformation("Updating name for product with ID: {Id}", id);

        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID: {Id} not found", id);
            return null;
        }

        try
        {
            product.Name = productName;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Updated name for product with ID: {Id}", id);

            var response = new ProductResponse(
                product.Id,
                product.Name,
                product.ProductCategories
                .Select(pc => new CategoryResponse(pc.Category.Id, pc.Category.Name)).ToList()
            );

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating name for product with ID: {Id}", id);
            return new ProductResponse(0, "", []);
        }
    }
}
