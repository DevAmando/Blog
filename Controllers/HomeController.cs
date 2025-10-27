using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class HomeController : ControllerBase
    {
        private readonly BlogDataContext _dbcontext;

        public HomeController(BlogDataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok("API is running...");
        }
    }
}
