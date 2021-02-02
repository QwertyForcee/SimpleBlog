using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlog.Models.Comments
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }
        [Required]
        [MaxLength(50)]
        public string Author { get; set; }
        public DateTime Created { get; set; }
    }
}
