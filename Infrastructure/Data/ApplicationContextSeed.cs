
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class ApplicationContextSeed
{
    public static async Task SeedAsync(ApplicationContext context)
    {
        if (context.Products.Any() == false)
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeededData/products.json");

            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            if (products == null)
            {
                return;
            } 
            else
            {
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            } 
        }

        if (context.DeliveryMethods.Any() == false)
        {
            var deliveryMethodsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeededData/deliveryMethods.json");

            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

            if(deliveryMethods == null)
            {
                return;
            }
            else
            {
                context.DeliveryMethods.AddRange(deliveryMethods);
                await context.SaveChangesAsync();
            }
        }
    }
}
