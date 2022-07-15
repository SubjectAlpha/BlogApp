using BlogApp.Data;
using BlogApp.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApp.Pages.User
{
    public class SignInModel : PageModel
    {
        private readonly ILogger _logger;
        private readonly BlogContext _context;

        public string ErrorMessage = string.Empty;

        public SignInModel(ILogger<SignInModel> logger, BlogContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            HttpContext.Session.SetString("loggedIn", "false"); //Remove the session value so that the user is forcibly logged out on accessing this page.
            ViewData["LoggedIn"] = false;
        }

        public IActionResult OnPost(string email, string password)
        {
            //If either parameter is null or an empty string or white space, give an error message and return the page.
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Both fields must be filled out";
                return Page();
            }

            try
            {
                //Attempt to authenticate the user, if it fails return the page with an error message.
                if (AccountHelper.ValidateAuthentication(_context, email, password))
                {
                    //If authentication is sucessful set the session string to true and redirect to the home page.
                    HttpContext.Session.SetString("loggedIn", "true");
                    return Redirect("/");
                }
                ErrorMessage = "Could not verify credentials";
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An exception ocurred. {ex.Message}";
                return Page();
            }
        }
    }
}
