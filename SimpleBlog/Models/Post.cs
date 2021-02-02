using SimpleBlog.Models.Comments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.Models
{
    public class Post
    {
        
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "";
        [Required]
        public string Body { get; set; } = "";
        public string Description { get; set; } = ""; 
        public string Tags { get; set; } = "";
        public string Category { get; set; } = "";

        public string Image { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;
        public List<MainComment> MainComments { get; set; }
    }
}
