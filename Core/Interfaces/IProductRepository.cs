using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IList<Product>> GetProductsAsync(string[]? brands, string[]? types, string? sort, string? search, int pageNumber, int entitiesPerPage);
    Task<IList<string>> GetBrandsAsync();
    Task<IList<string>> GetTypesAsync();
}
