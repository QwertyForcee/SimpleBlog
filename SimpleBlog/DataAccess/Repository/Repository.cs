using Microsoft.EntityFrameworkCore;
using SimpleBlog.Contexts;
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
    public class Repository : IRepository
    {
        private PostContext _ctx;

        public Repository(PostContext ctx)
        {
            _ctx = ctx;
        }
        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
        }
        public async Task<List<Post>> GetAllPosts()
        {
            return await _ctx.Posts
                .ToListAsync();
        }
        public async Task<IndexViewModel> GetAllPosts(int pageNumber)
        {
            int pageSize = 5;
            int skipAmount = pageSize * (pageNumber - 1);
            var query = await _ctx.Posts
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();
            return new IndexViewModel
            {
                PageNumber=pageNumber,
                NextPage= query.Count() > (skipAmount + pageSize),
                Posts = query
            };
        }
        public async Task<IndexViewModel> GetAllPosts(int pageNumber, string category)
        {
            int pageSize = 5;
            int skipAmount = pageSize * (pageNumber - 1);
            var query = _ctx.Posts.AsQueryable();
               

            if (!String.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.ToLower() == category.ToLower());
            }

            //query = await query.ToListAsync();
            //var query = await _ctx.Posts
            //    .Where(p => p.Category.ToLower() == category.ToLower()).Skip(skipAmount)
            //    .Take(pageSize).ToListAsync();
            int postsCount = query.Count();
            return new IndexViewModel {
                PageNumber = pageNumber,
                NextPage = (postsCount > (skipAmount + pageSize)),
                Category = category,
                PagesCount= (int)Math.Ceiling(postsCount*1.0/ pageSize),
                Posts = await query.Skip(skipAmount)
                .Take(pageSize).ToListAsync()
            };
        }

        public async Task<Post> GetPost(int id)
        {
            return await _ctx.Posts
                .Include(c=>c.MainComments)
                .ThenInclude(s=>s.SubComments)
                .FirstOrDefaultAsync(post => post.Id == id);
        }

        public void RemovePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id).Result);
        }

        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _ctx.SaveChangesAsync())>0;
        }

        public void AddSubComment(SubComment comment)
        {
            _ctx.SubComments.Add(comment);
        }
    }
}
