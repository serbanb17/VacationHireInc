// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

namespace VacationHireInc.Security.Interfaces
{
    public interface IHashingHelper
    {
        /// <summary>
        /// Applies extra data to the input and then computes the SHA256 hash
        /// </summary>
        /// <param name="input">
        /// Input string to be hashed
        /// </param>
        /// <returns>
        /// Salted and hashed string
        /// </returns>
        string SaltHash(string input);
    }
}
