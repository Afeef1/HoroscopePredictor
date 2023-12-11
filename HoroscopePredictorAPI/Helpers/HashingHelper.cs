using System.Security.Cryptography;
using System.Text;

namespace HoroscopePredictorAPI.Helpers
{
    public static class HashingHelper
    {

        public static string GetHashedPassword(string input)
        {
            SHA256 sha256Algorithm = SHA256.Create();
            byte[] hashedBytes = sha256Algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder hashedPasswordBuilder = new StringBuilder();
            for (int index = 0; index < hashedBytes.Length; index++)
            {
                hashedPasswordBuilder.Append(hashedBytes[index].ToString("x2"));
            }
            return hashedPasswordBuilder.ToString();
        }
    }
}
