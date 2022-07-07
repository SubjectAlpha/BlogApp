using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogApp.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Pages
{

    //This is the backend of our razor page. It handles requests similarly to a controller, but can handle requests directly from the page.
    public class IndexModel : PageModel
    {
        private readonly BlogContext _context; //Database context
        private readonly ILogger<IndexModel> _logger;

        public bool BloggerExists = true;
        public bool LoggedIn = false;
        public string GeneratedPassword = string.Empty;
        public IEnumerable<Post> Posts { get; set; }

        //The page constructor is called each time the page is requested.
        public IndexModel(ILogger<IndexModel> logger, BlogContext context)
        {
            _logger = logger;
            _context = context;

            BloggerExists = _context.Users.Any(); //Check to see if any bloggers exist in the database.
        }

        //OnGet will be called on GET HTTP requests.
        public async Task OnGet()
        {
            if(!BloggerExists)
            {
                //If no blogger exists generate a new 24 character long suggested password.
                GeneratedPassword = AccountHelper.GeneratePassword(24);
            }

            //See if the current session is logged in.
            bool loggedIn = AccountHelper.IsLoggedIn(HttpContext);
            
            ViewData["LoggedIn"] = loggedIn; //Store the value in ViewData for use across pages.
            LoggedIn = loggedIn; //Set the LoggedIn value for this page specifically.

            //Populate the posts in order of the date they were created, reverse the list so they display newest -> latest.
            this.Posts = _context.Posts.OrderBy(p => p.Created).Reverse();
        }

        //OnPost will be called on POST HTTP requests. This is meant to handle the registration form.
        public async Task<IActionResult> OnPost(string email, string password)
        {
            try
            {
                if (_context.Users.Where(u => u.Email == email).Any())
                {
                    //If a user with the supplied email exists, send a HTTP 409 error.
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }

                if (!AccountHelper.IsValidEmail(email))
                {
                    //If the email is invalid send a HTTP 400 error.
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }

                //Create a new user
                new Data.Entities.User().Create(_context, email, password);

                //Save database changes. (This is required after database changes, 
                await _context.SaveChangesAsync();

                //Redirect to the homepage.
                return Redirect("/");

            } catch (Exception e)
            {
                //Send HTTP 500 error
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}