using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class User : Base
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public User(string firstName, string lastName, string email = "")
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }
    }
}
