using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;

namespace Blog.Pages
{
    public class CategoriesModel : PageModel
    {
        private readonly BlogDataContext _context;

        public CategoriesModel(BlogDataContext context)
        {
            _context = context;
        }

        public List<CategoryViewModel> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            Categories = await _context
                .Categories
                .AsNoTracking()
                .Include(x => x.Posts)
                .Select(x => new CategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    PostsCount = x.Posts.Count
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int PostsCount { get; set; }
    }
}

