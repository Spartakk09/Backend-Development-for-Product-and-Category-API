namespace ProductsCategories.Services.Models.Product;

public record ProductCreateRequest(
    string Name,
    List<int> CategoryIds
);
