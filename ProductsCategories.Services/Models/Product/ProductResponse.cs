using ProductsCategories.Services.Models.Category;

namespace ProductsCategories.Services.Models.Product;

public record ProductResponse(
    int Id,
    string Name,
    List<CategoryResponse> Categories
);
