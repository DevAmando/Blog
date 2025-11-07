using System.Collections.Generic;

namespace Blog.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        
        public IList<Post> Posts { get; set; } = new List<Post>();
    }
}