using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogApp.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BlogContext _context;
        private readonly ILogger<IndexModel> _logger;

        public bool BloggerExists = true;
        public bool LoggedIn = false;
        public string GeneratedPassword = string.Empty;
        public List<Post> Posts { get; set; }

        public IndexModel(ILogger<IndexModel> logger, BlogContext context)
        {
            _logger = logger;
            _context = context;

            BloggerExists = _context.Users.Any();
        }

        public async Task OnGet()
        {
            if(!BloggerExists)
            {
                GeneratedPassword = AccountHelper.GeneratePassword(24);
            }

            bool loggedIn = AccountHelper.IsLoggedIn(HttpContext);

            ViewData["LoggedIn"] = loggedIn;
            LoggedIn = loggedIn;

            this.Posts = await _context.Posts.OrderBy(p => p.Created).Reverse().ToListAsync();
        }

        public async Task<IActionResult> OnPost(string email, string password)
        {
            try
            {
                if(BloggerExists)
                {
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }

                if (_context.Users.Where(u => u.Email == email).Any())
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }

                if (!AccountHelper.IsValidEmail(email))
                {
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }

                new Data.Entities.User().Create(_context, email, password);

                await _context.SaveChangesAsync();

                return Redirect("/");

            } catch (Exception e)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}