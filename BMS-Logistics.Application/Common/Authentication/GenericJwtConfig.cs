using BMS_Logistics.Application.Interfaces;
using BMS_Logistics.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BMS_Logistics.Application.Common.Authentication
{
    public class GenericJwtConfig
    {
        private readonly IDecryptHelper _encrypt;

        public GenericJwtConfig(IDecryptHelper encrypt)
        {
            _encrypt = encrypt;
        }

        public string GetToken(User user, IConfiguration configuration)
        {
            string secretKey = configuration["JwtSettings:SecretKey"]!;
            string issuer = configuration["JwtSettings:Issuer"]!;
            string audience = configuration["JwtSettings:Audience"]!;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                //new Claim("id", _encrypt.Encrypt(userInfo.Id.ToString())), // Create a new claim with the user's / customer's Id
                //new Claim("roleId", userInfo is User ? _encrypt.Encrypt(userInfo.RoleId.ToString()) : ""), // Create a new claim with the user's RoleId
                //new Claim("profile", userInfo is User ? _encrypt.Encrypt("Admin") : ""), // Create a new claim with the user's RoleId
                //new Claim("username", userInfo is User ? _encrypt.Encrypt(userInfo.Username) : ""), // Create a new claim with the user's RoleId
                //new Claim("passwordForgotten", userInfo is User ? _encrypt.Encrypt(userInfo.PasswordForgotten.ToString()) : ""), // Create a new claim with the info about the password forgotten
                //new Claim(JwtRegisteredClaimNames.Sub, userInfo is not User ? _encrypt.Encrypt(userInfo.Name) : _encrypt.Encrypt($"{userInfo.Name} {userInfo.Lastname}")), // Create a new claim with the user's / customer's name
                //new Claim(JwtRegisteredClaimNames.Email, _encrypt.Encrypt(userInfo.Email)), // Create a new claim with the user's / customer's email
                new Claim("id", _encrypt.Encrypt(user.Id.ToString())),
                new Claim("username", _encrypt.Encrypt(user.UserName!)),
                new Claim("name", _encrypt.Encrypt(user.Name!)),
                new Claim("lastname", _encrypt.Encrypt(user.SurName!)),
                new Claim("roleId", _encrypt.Encrypt(user.RoleId!.ToString())),
                new Claim("companyId", _encrypt.Encrypt(user.CompanyId.ToString())),
                new Claim("branchId", _encrypt.Encrypt(user.BranchId.ToString()))
            };

            var token = new JwtSecurityToken
                (
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static Token GetUserToken(ClaimsIdentity identity)
        {
            Token token = new();

            if (identity != null)
            {
                foreach (Claim claim in identity.Claims)
                {
                    if (claim.Type == "Id")
                        token.Id = int.Parse(claim.Value);
                }
            }

            return token;
        }

        // Method for Generating a Secure Refresh Token
        public string GenerateRefreshToken()
        {
            // Combine GUIDs to generate a long, secure token
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()) +
                   Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}
