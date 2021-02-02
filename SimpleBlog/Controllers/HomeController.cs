using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Contexts;
using SimpleBlog.DataAccess.FileManager;
using SimpleBlog.DataAccess.Repository;
using SimpleBlog.Models;
using SimpleBlog.Models.Comments;
using SimpleBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlog.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager)
        {   
            _repo = repo;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index(int pageNumber,string category)
        {
            if (pageNumber < 1) return RedirectToAction("Index", new { pageNumber = 1, category=category });
                var vm = await _repo.GetAllPosts(pageNumber,category);
                return View(vm);
        }
        public async Task<IActionResult> Post(int id)
        {
            var post = await _repo.GetPost(id);
            return View(post);
        }
        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            string mime = image.Substring(image.LastIndexOf('.')+1);
            return new FileStreamResult(_fileManager.ImageStream(image),$"image/{mime}");
        }
        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = vm.PostId });
            }

            var post = await _repo.GetPost(vm.PostId);
            if (vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();
                post.MainComments.Add(new MainComment
                {
                    Author=User.Identity.Name,
                    Message = vm.Message,
                    Created = DateTime.Now
                });
                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment()
                {
                    MainCommentId = vm.MainCommentId,
                    Author = vm.Author,
                    Message = vm.Message,
                    Created = DateTime.Now
                };
                _repo.AddSubComment(comment);

            }
            await _repo.SaveChangesAsync();
            return RedirectToAction("Post",new { id = vm.PostId });
        }   
    }
}
