using BlogApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BlogContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, BlogContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            _logger.Log(LogLevel.Critical, "Page accessed");
        }
    }
}