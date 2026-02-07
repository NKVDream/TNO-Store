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
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync(); // Сохраняем изменения асинхронно
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

    }
}
