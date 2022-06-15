using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Entities
{
    public class Post : Base
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
