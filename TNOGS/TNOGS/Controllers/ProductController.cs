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
    }
}
