using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories;

public class ProductRepository(Data.AppContext context) : Repository<Product>(context), IProductRepository
{
    public async Task<IList<Brand>> GetBrandsAsync()
    {
        var brands = await context.Products.Select(x => x.Brand).Distinct().ToListAsync();

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

    public async Task<IList<Core.Entities.Type>> GetTypesAsync()
    {
        var types = await context.Products.Select(x => x.Type).Distinct().ToListAsync();

        return types; 
    }
}
