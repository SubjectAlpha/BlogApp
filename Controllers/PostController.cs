using BlogApp.Data;
using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        public struct PostData
        {
            public string Title;
            public string Body;
        }

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

        [HttpPost("")]
        public async Task<bool> Create(Guid creatorId, [FromBody] PostData post)
        {
            try
            {
                var newPost = new Post()
                {
                    CreatedBy = creatorId,
                    Title = post.Title,
                    Body = post.Body,
                };

                _context.Posts.Add(newPost);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPatch("{id}")]
        public async Task<bool> Update(Guid id, [FromBody] PostData updatePost)
        {
            try
            {
                var post = _context.Posts.Single(x => x.Id == id);

                post.Title = updatePost.Title;
                post.Body = updatePost.Body;

                await _context.SaveChangesAsync();
                return true;
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
                var post = _context.Posts.Single(x => x.Id == id);
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
