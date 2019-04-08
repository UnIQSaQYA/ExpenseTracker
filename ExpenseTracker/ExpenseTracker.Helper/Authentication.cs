using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Helper
{
    public class Authentication
    {
        private const int saltByteSize = 16;
        private const int hashByteSize = 20;
        private const int pkbd2Iterations = 2000;
        private static byte[] salt = new byte[saltByteSize];
        private static byte[] hash = new byte[hashByteSize];

        // Function to convert plaintext password to hashed value.
        public static string HashText(string password)
        {
            // Use and dispose Random number generator after getting
            // a random byte for use as Salt value.
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                cryptoProvider.GetBytes(salt);
            }

            // Use and dispose hash value generator after getting hashed
            // password byte. `Pkbd2Iterations' parameter controls how slow
            // this function returns so as to deter Brute-force password cracking.
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, pkbd2Iterations))
            {
                hash = pbkdf2.GetBytes(hashByteSize);
            }

            // Combine the salt and hash values to one in order to store it in
            // one go.
            var hashByte = new byte[44];
            Array.Copy(salt, 0, hashByte, 0, saltByteSize);
            Array.Copy(hash, 0, hashByte, saltByteSize, hashByteSize);

            // Convert byte array to string and return the hash string.
            string hashpass = Convert.ToBase64String(hashByte);
            return hashpass;
        }

        // Function to check if given hash of entered password is same as hash
        // stored in database. Most operations are same as above function.
        public static bool CheckPass(string password, string hashVal)
        {
            bool flag = true;
            var hashBytes = Convert.FromBase64String(hashVal);
            salt = new byte[saltByteSize];

            Array.Copy(hashBytes, 0, salt, 0, saltByteSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, pkbd2Iterations))
            {
                hash = pbkdf2.GetBytes(hashByteSize);

                // Here, entered password hash is checked against hash from database.
                // If any value doesn't match, `false' is returned.
                for (var i = 0; i < hashByteSize; i++)
                {
                    if (hashBytes[i + saltByteSize] != hash[i])
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
    }
}
