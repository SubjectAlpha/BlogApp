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

        public SignInModel(ILogger<SignInModel> logger, BlogContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            HttpContext.Session.SetString("loggedIn", "false");
            ViewData["LoggedIn"] = false;
        }

        public IActionResult OnPost(string email, string password)
        {
            try
            {
                if(AccountHelper.ValidateAuthentication(_context, email, password))
                {
                    HttpContext.Session.SetString("loggedIn", "true");
                    return Redirect("/");
                }
                return Page();
            }
            catch (Exception)
            {
                return Page();
            }
        }
    }
}
