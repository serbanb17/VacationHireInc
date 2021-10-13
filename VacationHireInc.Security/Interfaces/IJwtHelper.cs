// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using System;

namespace VacationHireInc.Security.Interfaces
{
    public interface IJwtHelper
    {
        /// <summary>
        /// Create a jwt token that contain the id in the payload
        /// </summary>
        /// <see href="https://jwt.io/"/>
        string GetJwt(Guid id);

        /// <param name="token">Jwt token to be verified</param>
        /// <param name="isBearerFormat">Specifies if token starts with "Beared: "</param>
        /// <param name="id">If return value is true, contains the id from the token payload</param>
        /// <returns>true is token is valid; else return false</returns>
        bool IsJwtValid(string token, bool isBearerFormat, out Guid id);
    }
}
