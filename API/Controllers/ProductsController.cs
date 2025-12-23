using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class ProductsController(IProductRepository productsRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IList<Product>>> GetProducts([FromQuery] string[]? brands, string[]? types, string? sort, string? search)
    {
        return Ok(await productsRepository.GetProductsAsync(brands, types, sort, search));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await productsRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productsRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        productsRepository.Delete(product);

        if (await productsRepository.Complete())
        {
            return NoContent();
        }

        return BadRequest("There was a problem that occurred while deleting the product.");
    }

    [HttpPost]
    public async Task<ActionResult<Product>> AddProduct(Product product)
    {
        productsRepository.Add(product);

        if (await productsRepository.Complete())
        {
            return CreatedAtAction("GetProductById", new {id = product.Id}, product);
        }

        return BadRequest("There was a problem that occurred while creating the product.");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> EditProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
        {
            return BadRequest("It was not possible to update this product.");
        }

        productsRepository.Edit(product);

        if (await productsRepository.Complete())
        {
            return CreatedAtAction("GetProductById", new {id = product.Id}, product);
        }

        return BadRequest("There was a problem that occurred while updating the product.");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IList<string>>> GetBrands()
    {
        return Ok(await productsRepository.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IList<string>>> GetTypes()
    {
        return Ok(await productsRepository.GetTypesAsync());
    }

    private bool ProductExists(int id)
    {
        return productsRepository.Exists(id);
    }
}
