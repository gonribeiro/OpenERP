using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace OpenERP.Services.Auth
{
    public class PasswordService
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public static bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(storedPassword);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static (bool isValid, string ErrorMessage) ValidatePassword(string password)
        {
            if (password.Length < 6)
                return (false, "Password must be at least 6 characters long");

            if (password.Length > 50)
                return (false, "Password cannot exceed 50 characters");

            if (!Regex.IsMatch(password, @"[A-Z]")) // Check if the password contains at least one uppercase letter
                return (false, "Password must contain at least one uppercase letter");

            if (!Regex.IsMatch(password, @"[a-z]")) // Check if the password contains at least one lowercase letter
                return (false, "Password must contain at least one lowercase letter");

            if (!Regex.IsMatch(password, @"\d")) // Check if the password contains at least one numeric digit
                return (false, "Password must contain at least one numeric digit");

            if (!Regex.IsMatch(password, @"[!?@$%&#.]")) // Check if the password contains at least one special character !?@$%&
                return (false, "Password must contain at least one special character (!?@$%&#.)");

            return (true, string.Empty);
        }
    }
}
