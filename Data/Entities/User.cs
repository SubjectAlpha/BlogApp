using BlogApp.Utility;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Entities
{
    public class User : Base
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)] //Use this so EntityFramework knows that this is a password.
        public string HashedPassword { get; set; }

        [Required]
        public string Salt { get; set; }

        //This function will allow us to create and save a post in the database.
        //Take in the current database context, desired email, and unsecured password.
        public bool Create(BlogContext context, string email, string rawPassword)
        {
            try
            {
                //Make sure that the email supplied is a valid email. If not exit and send a false response.
                if (!AccountHelper.IsValidEmail(email))
                {
                    return false;
                }

                //Encrypt the password!
                var encryptedPassword = AccountHelper.EncryptPassword(rawPassword);

                //Assign values to the current object
                this.Email = email;
                this.HashedPassword = encryptedPassword.Hash;
                this.Salt = encryptedPassword.Salt;

                //Add the user to the database.
                context.Users.Add(this);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
