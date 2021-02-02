using SimpleBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlog.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Post> Posts{ get; set; }
        public bool NextPage { get; set; }
        public int PageNumber { get; set; }
        public int PagesCount{ get; set; }
        public string Category { get; set; }
    }
}
