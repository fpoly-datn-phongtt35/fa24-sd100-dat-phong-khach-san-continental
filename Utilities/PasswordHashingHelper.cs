using System.Text;
using System.Security.Cryptography;

namespace Utilities
{
    public static class PasswordHashingHelper
    {
        public static string RandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random random = new Random();
            char[] password = new char[6];

            for (int i = 0; i < password.Length; i++)
            {
                password[i] = chars[random.Next(chars.Length)];
            }

            return new string(password);
        }
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
