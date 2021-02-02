using SimpleBlog.Models;
using SimpleBlog.Models.Comments;
using SimpleBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlog.DataAccess.Repository
{
    public interface IRepository
    {
        public Task<Post> GetPost(int id);
        public Task<List<Post>> GetAllPosts();
        public Task<IndexViewModel> GetAllPosts(int pageNumber);
        public Task<IndexViewModel> GetAllPosts(int pageNumber, string Category);
        public void AddPost(Post post);
        public void UpdatePost(Post post);
        public void RemovePost(int id);
        void AddSubComment(SubComment comment); 
        Task<bool> SaveChangesAsync();
    }
}
