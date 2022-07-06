using BlogApp.Utility;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Entities
{
    public class User : Base
    {
        /*
         * 
         */
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string HashedPassword { get; set; }

        [Required]
        public string Salt { get; set; }

        public bool Create(BlogContext context, string email, string rawPassword)
        {
            try
            {
                if (!AccountHelper.IsValidEmail(email)) { return false; }
                var encryptedPassword = AccountHelper.EncryptPassword(rawPassword);

                this.Email = email;
                this.HashedPassword = encryptedPassword.Hash;
                this.Salt = encryptedPassword.Salt;

                context.Users.Add(this);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Update(BlogContext context)
        {
            context.Users.Update(this);
        }
    }
}
