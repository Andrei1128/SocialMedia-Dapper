using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SocialMedia_Dapper.Utilities
{
    class AuthUtil
    {
        private readonly IConfiguration _config;
        private string TokenKey;
        public AuthUtil(IConfiguration config)
        {
            _config = config;
            TokenKey = config.GetValue<string>("AppSettings:TokenKey");
        }
        public string EncodePassword(string password)
        {
            string base64HashedPasswordBytes;
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                base64HashedPasswordBytes = Convert.ToBase64String(hashedPasswordBytes);
            }
            return base64HashedPasswordBytes;
        }
        public string CreateToken(int userId)
        {
            Claim[] claims = new Claim[] {
                new Claim("userId", userId.ToString())
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(TokenKey)
            );
            SigningCredentials credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(3)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}