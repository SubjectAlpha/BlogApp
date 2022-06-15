using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Entities
{
    public class User : Base
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string HashedPassword { get; set; }

        [Required]
        public string Salt { get; set; }
    }
}
