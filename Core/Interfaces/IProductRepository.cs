using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IList<Product>> GetProductsAsync(string[]? brands, string[]? types, string? sort, string? search);
    Task<IList<Brand>> GetBrandsAsync();
    Task<IList<Entities.Type>> GetTypesAsync();
}
