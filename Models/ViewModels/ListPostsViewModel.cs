namespace Blog.Models.ViewModels;
using System;

public class ListPostsViewModel
    {
     public int Id { get; set; }
     public string Title { get; set; } = string.Empty;
     public string Summary { get; set; } = string.Empty;
     public string Slug { get; set; } = string.Empty;
     public DateTime LastUpdateDate { get; set; }
     public string Category { get; set; } = string.Empty;
     public string Author { get; set; } = string.Empty;
    }