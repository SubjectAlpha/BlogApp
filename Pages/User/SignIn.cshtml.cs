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
        }

        public void OnPost(string email, string password)
        {
            try
            {
                if(AccountHelper.IsValidEmail(email))
                {
                    var user = _context.Users.Single(x => x.Email == email);

                    if(AccountHelper.VerifyPassword(password, new EncryptedPair { Hash = user.HashedPassword, Salt = user.Salt }))
                    {
                        Response.Cookies.Append("BlogAppAuth", AccountHelper.GenerateCookie(user));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
