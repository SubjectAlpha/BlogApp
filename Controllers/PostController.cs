using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogApp.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BlogContext _context;

        public PostController(BlogContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public Post Get(Guid id)
        {
            try
            {
                return _context.Posts.Single(x => x.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> Create([FromBody]KeyValuePair<string, string> post)
        {
            try
            {
                var loggedIn = HttpContext.Session.GetString("loggedIn");
                if (!string.IsNullOrEmpty(loggedIn) && string.Compare(loggedIn, "true") == 0)
                {
                    var newPost = new Post()
                    {
                        Title = post.Key,
                        Body = post.Value,
                    };

                    _context.Posts.Add(newPost);
                    await _context.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPatch("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<bool> Update(Guid id, [FromBody]KeyValuePair<string, string> updatePost)
        {
            try
            {
                var loggedIn = HttpContext.Session.GetString("loggedIn");
                if (!string.IsNullOrEmpty(loggedIn) && string.Compare(loggedIn, "true") == 0)
                {
                    var post = _context.Posts.Single(x => x.Id == id);

                    post.Title = updatePost.Key;
                    post.Body = updatePost.Value;

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var loggedIn = HttpContext.Session.GetString("loggedIn");
                if (!string.IsNullOrEmpty(loggedIn) && string.Compare(loggedIn, "true") == 0)
                {
                    var post = _context.Posts.Single(x => x.Id == id);
                    _context.Posts.Remove(post);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
