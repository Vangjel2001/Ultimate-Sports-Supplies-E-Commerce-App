using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories;

public class ProductsRepository(Data.ApplicationContext context) : Repository<Product>(context), IProductsRepository
{
    public async Task<IList<string>> GetBrandsAsync()
    {
        var brands = await context.Products.Select(x => x.Brand.ToString()).Distinct().ToListAsync();

        return brands; 
    }

    public async Task<IList<Product>> GetProductsAsync(string[]? brands, string[]? types, string? sort, string? search, int pageNumber, int entitiesPerPage)
    {
        var query = context.Products.AsQueryable();

        if (brands != null && brands.Count() > 0)
        {
            /*
                foreach (var brand in brands)
                {
                    query = query.Where(x => x.Brand.GetType().ToString() == brand);
                }
            */
            
            var parsedBrands = brands
            .Select(b => Enum.TryParse<Brand>(b, true, out var r) ? r : (Brand?)null)
            .Where(b => b.HasValue)
            .Select(b => b!.Value)
            .ToList();

            query = query.Where(x => parsedBrands.Contains(x.Brand));
        }

        if (types != null && types.Count() > 0)
        {
            var parsedTypes = types
            .Select(t => Enum.TryParse<Core.Entities.Type>(t, true, out var result) ? result : (Core.Entities.Type?)null)
            .Where(t => t.HasValue)
            .Select(t => t!.Value)
            .ToList();

            query = query.Where(x => parsedTypes.Contains(x.Type));
        }

        if (search.IsNullOrEmpty() == false)
        {
            
            query = query.Where(x => x.Name.Contains(search) || x.Description.Contains(search) || 
                /*x.Brand.ToString().Contains(search) || x.Type.ToString().Contains(search) || */ x.Price.ToString().Contains(search) 
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

        query = query.Include(x => x.ProductPictures);

        var products = await query.Skip((pageNumber - 1) * entitiesPerPage)
                       .Take(entitiesPerPage).ToListAsync();

        return products;
    }

    public async Task<Product?> GetProductWithPicturesByIdAsync(int id)
    {
        return await context.Products
        .Include(p => p.ProductPictures)
        .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IList<string>> GetTypesAsync()
    {
        var types = await context.Products.Select(x => x.Type.ToString()).Distinct().ToListAsync();

        return types; 
    }
}
