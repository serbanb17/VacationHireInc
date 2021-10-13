// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using VacationHireInc.Security.Interfaces;

namespace VacationHireInc.Security
{
    public class HashingHelper : IHashingHelper
    {
        private byte[] _salt;
        HashAlgorithm _hashAlgorithm;

        /// <summary>
        /// Implements IHashingHelper
        /// </summary>
        /// <param name="salt">
        /// Array of bytes that will be applied to the input before hashing
        /// </param>
        public HashingHelper(byte[] salt)
        {
            _salt = salt;
            _hashAlgorithm = new SHA256Managed();
        }

        public string SaltHash(string input)
        {
            List<byte> saltedInputBytes = Encoding.Unicode.GetBytes(input).ToList();
            saltedInputBytes.AddRange(_salt);
            byte[] resultBytes = _hashAlgorithm.ComputeHash(saltedInputBytes.ToArray());
            string result = Encoding.Unicode.GetString(resultBytes);
            return result;
        }
    }
}
