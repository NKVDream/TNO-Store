/*
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNOGS.Data;

namespace TNOGS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;//нужно для подключения моделей и связи с БД
        public ProductController(AppDbContext context)//тоже самое
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetProducts()
        {
            return Ok(_context.Products.ToList());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)//async С async: Поток "освобождается" на время ожидания.
        {
            // await говорит: "подожди здесь, пока база ответит, но не блокируй поток"
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult<Products>> CreateProduct(CreateProducts productDto)
        {
            var createdProduct = await _productService.CreateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

    }
}
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TNOGS.Models;
using TNOGS.Data;

namespace TNOGS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Types)  // Загружаем связанные Types
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Types)  // Загружаем связанные Types
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return NotFound(new { message = $"Product with id {id} not found" });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Products>> CreateProduct([FromBody] Products product)
        {
            try
            {
                // Проверяем, существует ли указанный TypeId
                var typeExists = await _context.Types.AnyAsync(t => t.Id == product.TypeId);
                if (!typeExists)
                {
                    return BadRequest(new { message = $"Type with id {product.TypeId} not found" });
                }

                // Добавляем продукт
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // Загружаем связанный тип для ответа
                await _context.Entry(product)
                    .Reference(p => p.Types)
                    .LoadAsync();

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Products product)
        {
            if (id != product.Id)
            {
                return BadRequest(new { message = "ID in URL does not match ID in body" });
            }

            try
            {
                // Проверяем существование продукта
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(new { message = $"Product with id {id} not found" });
                }

                // Проверяем существование типа
                var typeExists = await _context.Types.AnyAsync(t => t.Id == product.TypeId);
                if (!typeExists)
                {
                    return BadRequest(new { message = $"Type with id {product.TypeId} not found" });
                }

                // Обновляем свойства
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.TypeId = product.TypeId;
                existingProduct.Price = product.Price;
                existingProduct.Availability = product.Availability;
                existingProduct.Quantity = product.Quantity;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product {Id}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound(new { message = $"Product with id {id} not found" });
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {Id}", id);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // GET: api/products/type/5
        [HttpGet("type/{typeId}")]
        public async Task<ActionResult<IEnumerable<Products>>> GetProductsByType(int typeId)
        {
            try
            {
                // Проверяем существование типа
                var typeExists = await _context.Types.AnyAsync(t => t.Id == typeId);
                if (!typeExists)
                {
                    return NotFound(new { message = $"Type with id {typeId} not found" });
                }

                var products = await _context.Products
                    .Include(p => p.Types)
                    .Where(p => p.TypeId == typeId)
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products by type {TypeId}", typeId);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // GET: api/products/search?term=ноутбук
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Products>>> SearchProducts([FromQuery] string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return await GetProducts();
                }

                var products = await _context.Products
                    .Include(p => p.Types)
                    .Where(p => p.Name.Contains(term) ||
                               (p.Description != null && p.Description.Contains(term)))
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching products with term {SearchTerm}", term);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // GET: api/products/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<Products>>> GetAvailableProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Types)
                    .Where(p => p.Availability == true && p.Quantity > 0)
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available products");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}