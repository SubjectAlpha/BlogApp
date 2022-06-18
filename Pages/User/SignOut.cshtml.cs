using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApp.Pages.User
{
    public class SignOutModel : PageModel
    {
        public IActionResult OnGet()
        {
            Response.Cookies.Append("BlogAppAuth", string.Empty);
            return Redirect("/");
        }
    }
}
