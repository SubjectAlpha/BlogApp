using BlogApp.Data;
using BlogApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApp.Pages
{
    public class PostModel : PageModel
    {
        private readonly BlogContext _context;
        public Post post;

        public PostModel(BlogContext context)
        {
            _context = context;
        }

        public void OnGet(Guid id)
        {
            this.post = _context.Posts.Single(x => x.Id == id);
        }
    }
}
