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

    public async Task<IList<Product>> GetProductsAsync(string[]? brands, string[]? types, string? sort, string? search)
    {
        var query = context.Products.AsQueryable();

        if (brands != null && brands.Count() > 0)
        {
            foreach (var brand in brands)
            {
                query.Where(x => x.Brand.ToString() == brand);
            }
        }

        if (types != null && types.Count() > 0)
        {
            foreach (var type in types)
            {
                query.Where(x => x.Type.ToString() == type);
            }
        }

        query = sort switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            "ageAsc" => query.OrderBy(x => x.ArrivalDate),
            "ageDesc" => query.OrderByDescending(x => x.ArrivalDate),
            _ => query.OrderBy(x => x.Name)
        };

        if (search.IsNullOrEmpty() == false)
        {
            query = query.Where(x => x.Name.Contains(search) || x.Description.Contains(search) || 
                x.Brand.ToString().Contains(search) || x.Type.ToString().Contains(search) || x.Price.ToString().Contains(search) 
                || x.StockLevel.ToString().Contains(search));
        }

        return await query.ToListAsync();
    }

    public async Task<IList<string>> GetTypesAsync()
    {
        var types = await context.Products.Select(x => x.Type.ToString()).Distinct().ToListAsync();

        return types; 
    }
}
