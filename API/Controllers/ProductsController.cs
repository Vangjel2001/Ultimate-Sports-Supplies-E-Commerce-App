using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class ProductsController(IProductRepository productRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IList<Product>>> GetProducts(string[]? brands, string[]? types, string? sort, string? search)
    {
        return Ok(await productRepository.GetProductsAsync(brands, types, sort, search));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    private bool ProductExists(int id)
    {
        return productRepository.Exists(id);
    }
}
