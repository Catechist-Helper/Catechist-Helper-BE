using System.Text;

namespace CatechistHelper.Infrastructure.Utils
{
    public static class PasswordUtil
    {
        public static string HashPassword(string rawPassword)
        {
            byte[] bytes = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(rawPassword));
            return Convert.ToBase64String(bytes);
        }
        public static bool VerifyPassword(string rawPassword, string storedHash)
        {
            // Hash the provided raw password using the same HashPassword function
            string hashedPassword = HashPassword(rawPassword);

            // Compare the hashed password with the stored hash
            return hashedPassword == storedHash;
        }


    }
}