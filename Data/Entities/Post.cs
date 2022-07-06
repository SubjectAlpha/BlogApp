using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Entities
{
    public class Post : Base
    {
        /*
         * Here we will create variables to store the title and the body of our post.
         */

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
