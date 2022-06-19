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

        [HttpPost("Create")]
        public async Task<bool> Create([FromBody]KeyValuePair<string, string> post)
        {
            try
            {
                if(AccountHelper.VerifyUserFromCookie(_context, Request.Cookies["BlogAppAuth"] ?? string.Empty))
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
        public async Task<bool> Update(Guid id, [FromBody]KeyValuePair<string, string> updatePost)
        {
            try
            {
                if (AccountHelper.VerifyUserFromCookie(_context, Request.Cookies["BlogAppAuth"] ?? string.Empty))
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
        public async Task<bool> Delete(Guid id)
        {
            try
            {
                if (AccountHelper.VerifyUserFromCookie(_context, Request.Cookies["BlogAppAuth"] ?? string.Empty))
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
