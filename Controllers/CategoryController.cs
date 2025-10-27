using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Blog.Models.ViewModels;

namespace Blog.Controllers
{
    [ApiController]
    [Route("v1")]
    public class CategoryController : ControllerBase
    {
        private readonly BlogDataContext _dbcontext;

        public CategoryController(BlogDataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet("categories/{id:int}")]
        public async Task<IActionResult> GetIdAsync([FromRoute] int id)
        {
            try
            {
                var category = await _dbcontext.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteudo nao encontrado"));

                return Ok(new ResultViewModel<Category>(category));

            }
            catch (System.Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha Interna no Servidor"));
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var categories = await _dbcontext.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (System.Exception)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Falha Interna no Servidor"));
            }
        }

        [HttpPost("categories")]
        public async Task<IActionResult> PostAsync([FromBody] EditorCategoryViewModel model)
        {
            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug
                };

                _dbcontext.Categories.Add(category);
                await _dbcontext.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
            catch (System.Exception )
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha no create"));
            }
        }

        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> PutAsync([FromBody] EditorCategoryViewModel model, [FromRoute] int id)
        {
            try
            {
                var category = await _dbcontext.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteudo nao encontrado"));

                category.Name = model.Name;
                category.Slug = model.Slug;

                _dbcontext.Categories.Update(category);
                await _dbcontext.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = ex.InnerException?.Message ?? ex.Message });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha Interna"));
            }
        }

        [HttpDelete("categories/{id:int}")]
        public async Task<IActionResult> DeleteIdAsync([FromRoute] int id)
        {
            try
            {
                var category = await _dbcontext.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound();

                _dbcontext.Categories.Remove(category);
                await _dbcontext.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha Interna"));
            }
        }
    }
}
