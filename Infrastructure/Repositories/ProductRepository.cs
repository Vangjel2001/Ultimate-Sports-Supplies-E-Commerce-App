using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories;

public class ProductRepository(Data.ApplicationContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<IList<string>> GetBrandsAsync()
    {
        var brands = await context.Products.Select(x => x.Brand.ToString()).Distinct().ToListAsync();

        return brands; 
    }

    public async Task<IList<Product>> GetProductsAsync(string[]? brands, string[]? types, string? sort, string? search, int pageNumber = 1, int entitiesPerPage = 6)
    {
        var query = context.Products.AsQueryable();

        if (brands != null && brands.Count() > 0)
        {
            foreach (var brand in brands)
            {
                query = query.Where(x => x.Brand.ToString() == brand);
            }
        }

        if (types != null && types.Count() > 0)
        {
            foreach (var type in types)
            {
                query = query.Where(x => x.Type.ToString() == type);
            }
        }

        if (search.IsNullOrEmpty() == false)
        {
            query = query.Where(x => x.Name.Contains(search) || x.Description.Contains(search) || 
                x.Brand.ToString().Contains(search) || x.Type.ToString().Contains(search) || x.Price.ToString().Contains(search) 
                || x.StockLevel.ToString().Contains(search));
        }

        query = sort switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            "ageAsc" => query.OrderBy(x => x.ArrivalDate),
            "ageDesc" => query.OrderByDescending(x => x.ArrivalDate),
            _ => query.OrderBy(x => x.Name)
        };

        var products = await query.Skip((pageNumber - 1) * entitiesPerPage)
                       .Take(entitiesPerPage).ToListAsync();

        return products;
    }

    public async Task<IList<string>> GetTypesAsync()
    {
        var types = await context.Products.Select(x => x.Type.ToString()).Distinct().ToListAsync();

        return types; 
    }
}
