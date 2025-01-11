using ImageStoreBase.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ImageStoreBase.Api.Infrastructure
{
    public sealed class TokenProvider(IConfiguration configuration)
    {
        public async Task<string> CreateUserTokenAsync(User user, List<string>? userRoles)
        {
            // Khóa bí mật và cấu hình signing
            string secretKey = configuration["JwtSettings:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Tạo danh sách claims
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            // Thêm các roles vào claims
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Tạo token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JwtSettings:DurationInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["JwtSettings:Issuer"],
                Audience = configuration["JwtSettings:Audience"]
            };

            // Tạo token
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);

            // Trả về chuỗi token
            return handler.WriteToken(token);
        }
    }
}
