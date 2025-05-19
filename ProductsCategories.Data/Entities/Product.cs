namespace ProductsCategories.Data.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<ProductCategory> ProductCategories { get; set; } = [];
}
