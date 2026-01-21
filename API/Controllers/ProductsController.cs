using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class ProductsController(IProductsRepository productsRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] string[]? brands, [FromQuery] string[]? types, 
    string? sort, string? search, int pageNumber, int entitiesPerPage)
    {
        // if (entitiesPerPage > 50)
        // {
        //     entitiesPerPage = 50;
        // }
        // else if (entitiesPerPage < 6)
        // {
        //     entitiesPerPage = 6;
        // }

        var products = await productsRepository.GetProductsAsync(brands, types, sort, search, pageNumber, 
        entitiesPerPage);

       var allProducts = await productsRepository.GetAllAsync();

        var productPagination = new Pagination<Product>(pageNumber, entitiesPerPage, allProducts.Count, products);

        return Ok(productPagination);
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
