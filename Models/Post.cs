using System;
using System.Collections.Generic;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Body { get; set; }
        public string? Slug { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow;

     
        public int CategoryId { get; set; }  // chave estrangeira
        public Category? Category { get; set; }

       
        public int AuthorId { get; set; }  // chave estrangeira
        public User? Author { get; set; }

        
        public List<Tag> Tags { get; set; } = new();
    }
}
