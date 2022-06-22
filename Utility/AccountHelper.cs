using BlogApp.Data;
using BlogApp.Data.Entities;
using System.Security.Cryptography;
using System.Text;

namespace BlogApp.Utility
{
    public struct EncryptedPair
    {
        public string Hash;
        public string Salt;
    }

    public class AccountHelper
    {
        public const string CookieSeparationSequence = "!$%#!";
        private const string Lower = @"abcdefghijklmnopqrstuvwxyz";
        private const string Upper = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits = @"1234567890";
        private const string SpecialChars = @"~!@#$%^&*()-_=+[{]}\|;"":',<.>/?";
        private const int SaltSize = 16, HashSize = 24, HashIter = 10000;

        public static string GeneratePassword(int requiredLength)
        {
            var source =
                $"{Lower}{Upper}{Digits}{SpecialChars}".ToCharArray();
            var crypto = new RSACryptoServiceProvider();
            var result = new StringBuilder(requiredLength);

            while (result.Length < requiredLength)
            {
                var data = crypto.Encrypt(new byte[1], false);

                if (data[0] < (source.Length * 2))
                {
                    var currentValue = data[0] % source.Length;
                    result.Append(source[currentValue]);
                }
            }

            return result.ToString();
        }

        public static EncryptedPair EncryptPassword(string password)
        {
            var salt = new RSACryptoServiceProvider().Encrypt(new byte[SaltSize], false);
            var hash = new Rfc2898DeriveBytes(password, salt, HashIter).GetBytes(HashSize);
            return new EncryptedPair { Hash = Convert.ToBase64String(hash), Salt = Convert.ToBase64String(salt) };
        }
        
        public static bool VerifyPassword(string password, EncryptedPair encryptionInfo)
        {
            var testArray = new Rfc2898DeriveBytes(password, Convert.FromBase64String(encryptionInfo.Salt), HashIter).GetBytes(HashSize);
            var unencodedHash = Convert.FromBase64String(encryptionInfo.Hash);
            for (int i = 0; i < HashSize; i++)
            {
                if (testArray[i] != unencodedHash[i])
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
            var trimmedEmail = email.Trim();
            if(trimmedEmail.EndsWith("."))
            {
                return false;
            }

            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == trimmedEmail;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsLoggedIn(HttpContext httpContext)
        {
            var sessionLoggedIn = httpContext.Session.GetString("loggedIn");
            return (!string.IsNullOrEmpty(sessionLoggedIn) && string.Compare(sessionLoggedIn, "true") == 0);
        }
    }
}
