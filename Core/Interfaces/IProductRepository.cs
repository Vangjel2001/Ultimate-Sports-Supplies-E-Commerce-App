using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<IList<Product>> GetProductsAsync(string? brand, string? type, string? sort);
    Task<Product?> GetProductByIdAsync(int id);
    Task<IList<string>> GetBrandsAsync();
    Task<IList<string>> GetTypesAsync();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool ProductExists(int id);
    Task<bool> SaveChangesAsync();
}
