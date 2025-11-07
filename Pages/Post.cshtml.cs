using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;

namespace Blog.Pages
{
    public class PostModel : PageModel
    {
        private readonly BlogDataContext _context;

        public PostModel(BlogDataContext context)
        {
            _context = context;
        }

        public Post? Post { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await _context
                .Posts
                .AsNoTracking()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (Post == null)
            {
                return Page();
            }

            return Page();
        }
    }
}

