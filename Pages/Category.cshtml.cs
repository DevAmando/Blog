using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models.ViewModels;

namespace Blog.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly BlogDataContext _context;

        public CategoryModel(BlogDataContext context)
        {
            _context = context;
        }

        public string? CategoryName { get; set; }
        public List<ListPostsViewModel> Posts { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            var category = await _context
                .Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Slug == slug);

            if (category == null)
            {
                return Page();
            }

            CategoryName = category.Name;

            Posts = await _context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Where(x => x.Category != null && x.Category.Slug == slug)
                .OrderByDescending(x => x.LastUpdateDate)
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title ?? "Sem título",
                    Summary = x.Summary ?? "",
                    Slug = x.Slug ?? "",
                    Category = category.Name,
                    LastUpdateDate = x.LastUpdateDate,
                    Author = x.Author != null ? x.Author.Name ?? "Autor desconhecido" : "Autor desconhecido"
                })
                .ToListAsync();

            return Page();
        }
    }
}

