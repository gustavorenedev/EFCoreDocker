using EFCoreSqlServer.Context;
using EFCoreSqlServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSqlServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
    {
        return Ok(await _context.Products.ToListAsync());
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        if (ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] Product productFromJson)
    {
        if (ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null) return NotFound();

        product.Name = productFromJson.Name;
        product.Description = productFromJson.Description;
        product.Price = productFromJson.Price;

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return NotFound();

        _context.Remove(product);
        await _context.SaveChangesAsync();
        return Ok(product);
    }
}
