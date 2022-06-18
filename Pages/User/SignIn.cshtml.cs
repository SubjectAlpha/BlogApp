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
            Response.Cookies.Append("BlogAppAuth", string.Empty);
            ViewData["LoggedIn"] = false;
        }

        public IActionResult OnPost(string email, string password)
        {
            try
            {
                if(AccountHelper.ValidateAuthentication(_context, email, password))
                {
                    var user = _context.Users.Single(x => x.Email == email);
                    var newCookie = AccountHelper.GenerateCookie(email, user.HashedPassword);
                    Response.Cookies.Append("BlogAppAuth", newCookie);
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
