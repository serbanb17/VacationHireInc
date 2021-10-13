// Vacation Hire Inc.
// No copyright for this product. Intended for educational purposes.
// Feel free to modify / share the code!

using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using VacationHireInc.Security.Interfaces;

namespace VacationHireInc.Security
{
    public class JwtHelper : IJwtHelper
    {
        private const string BearerPrefix = "Bearer ";
        private byte[] _key;
        private string _issuer;
        private uint _expirationSeconds;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        /// <summary>
        /// Class to handle creation and validation of jwt tokens. Implements IJwtHelper
        /// </summary>
        /// <param name="key">
        /// Array of bytes that will be used to generate and validate jwt tokens
        /// </param>
        /// <param name="issuer">
        /// Issuer that will be added in the payload of jwt token
        /// </param>
        /// <param name="expirationSeconds">
        /// Number of seconds, after creation, until the token becomes invalid
        /// </param>
        public JwtHelper(byte[] key, string issuer, uint expirationSeconds)
        {
            _key = key;
            _issuer = issuer;
            _expirationSeconds = expirationSeconds;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public string GetJwt(Guid id)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(_key);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(claims: new [] { new Claim("Id", id.ToString()) },
                                                        issuer: _issuer,
                                                        expires: DateTime.Now.AddSeconds(_expirationSeconds),
                                                        signingCredentials: signingCredentials);

            string tokenString = _jwtSecurityTokenHandler.WriteToken(token);
            return tokenString;
        }

        public bool IsJwtValid(string token, bool isBearerFormat, out Guid id)
        {
            id = new Guid();

            if (string.IsNullOrWhiteSpace(token))
                return false;

            string cleanToken = token;
            if (isBearerFormat && cleanToken.StartsWith(BearerPrefix))
                cleanToken = cleanToken.Substring(BearerPrefix.Length);

            bool isValid = true;
            try
            {
                _jwtSecurityTokenHandler.ValidateToken(cleanToken, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = _issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(_key)
                }, out SecurityToken securityToken);

                string idStr = ((JwtSecurityToken)securityToken)?.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (idStr != null)
                    id = new Guid(idStr);
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
