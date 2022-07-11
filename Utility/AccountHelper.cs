using BlogApp.Data;
using BlogApp.Data.Entities;
using System.Security.Cryptography;
using System.Text;

namespace BlogApp.Utility
{
    public class AccountHelper
    {
        private const string Lower = @"abcdefghijklmnopqrstuvwxyz";
        private const string Upper = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits = @"1234567890";
        private const string SpecialChars = @"~!@#$%^&*()-_=+[{]}\|;"":',<.>/?";
        private const int SaltSize = 16, HashSize = 24, HashIter = 10000;

        //Create a struct to hold the values in a named pair
        public struct EncryptedPair
        {
            public string Hash;
            public string Salt;
        }

        //This method will securely generate a password for the defined character set with no modulo bias.
        public static string GeneratePassword(int requiredLength)
        {
            var source =
                $"{Lower}{Upper}{Digits}{SpecialChars}".ToCharArray(); // Load all allowable characters into single array
            var crypto = new RSACryptoServiceProvider(); //Instantiate the cryptographic service provider.
            var result = new StringBuilder(requiredLength); //This will allow us to create a string easily through the iterations.
            var emptyArr = Array.Empty<byte>(); //Instantiate outside of the loop for increased performance.

            while (result.Length < requiredLength)
            {
                var data = crypto.Encrypt(emptyArr, false); //Generate cryptographically secure random

                if (data[0] < (source.Length * 2)) //Ensure that the random generated is at least twice as big as the source array. This is what combats modulo bias.
                {
                    var currentValue = data[0] % source.Length; //Take the remainder of our character against the character array
                    result.Append(source[currentValue]); //Choose a character from the source array and append it to the string builder.
                }
            }

            return result.ToString(); //Send the result of the string builder.
        }

        public static EncryptedPair EncryptPassword(string password)
        {
            var salt = new RSACryptoServiceProvider().Encrypt(new byte[SaltSize], false); //Generate a new salt using the RSA algorithm.
            var hash = new Rfc2898DeriveBytes(password, salt, HashIter).GetBytes(HashSize); //Salt and hash the password.
            return new EncryptedPair { Hash = Convert.ToBase64String(hash), Salt = Convert.ToBase64String(salt) }; //Return the encrypted pair, the values have also been base64 encoded.
        }
        
        public static bool VerifyPassword(string password, EncryptedPair encryptionInfo)
        {
            var testArray = new Rfc2898DeriveBytes(password, Convert.FromBase64String(encryptionInfo.Salt), HashIter).GetBytes(HashSize); //Unencode the stored hash, and then generate a new hashed password based on the password supplied.
            var unencodedHash = Convert.FromBase64String(encryptionInfo.Hash);
            for (int i = 0; i < HashSize; i++)
            {
                if (testArray[i] != unencodedHash[i]) //Loop through each character of the hashed password and ensure they match what was expected.
                    return false;
            }
            return true;
        }

        public static bool ValidateAuthentication(BlogContext context, string email, string password)
        {
            if (IsValidEmail(email))
            {
                var user = context.Users.Single(x => x.Email == email);

                if (VerifyPassword(password, new EncryptedPair { Hash = user.HashedPassword, Salt = user.Salt }))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim(); //Remove whitespace
            if(trimmedEmail.EndsWith("."))
            {
                return false;
            }

            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email); //This will throw an exception if the email address is invalid.
                return mailAddress.Address == trimmedEmail; //Ensure the emails are the same.
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsLoggedIn(HttpContext httpContext)
        {
            //Check the session to see if the logged in string is set to true.
            var sessionLoggedIn = httpContext.Session.GetString("loggedIn");
            return (!string.IsNullOrEmpty(sessionLoggedIn) && string.Compare(sessionLoggedIn, "true") == 0);
        }
    }
}
