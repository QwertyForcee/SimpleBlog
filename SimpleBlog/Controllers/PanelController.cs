using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.DataAccess.FileManager;
using SimpleBlog.DataAccess.Repository;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBlog.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PanelController:Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public PanelController(IRepository repo,IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }

        public IActionResult Index()
        {
            var posts = _repo.GetAllPosts().Result;
            return View(posts);
        }
       
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var post =await  _repo.GetPost(id.Value);
                return View(new PostViewModel()
                {
                    Title = post.Title,
                    Body = post.Body,
                    Id = post.Id,
                    CurrentImage = post.Image,
                    Tags = post.Tags,
                    Category = post.Category,
                    Description = post.Description
                }) ;
            }
            else return View(new PostViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            var post = new Post
            {
                Title = vm.Title,
                Body = vm.Body,
                Id = vm.Id,
                Description=vm.Description,
                Tags=vm.Tags,
                Category=vm.Category
                
            };
            if (vm.Image == null)
            {
                post.Image = vm.CurrentImage;
            }
            else
            {
                post.Image = await _fileManager.SaveImage(vm.Image);
            }

            if (post.Id > 0)
            {
                _repo.UpdatePost(post);
            }
            else _repo.AddPost(post);
            if (await _repo.SaveChangesAsync())
                return RedirectToAction("Index");
            else return View(post);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            _repo.RemovePost(id.Value);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
