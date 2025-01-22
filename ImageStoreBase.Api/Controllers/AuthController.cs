using Azure.Core;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.GenericDTO;
using ImageStoreBase.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace ImageStoreBase.Api.Controllers
{
    public class AuthController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenProvider _tokenProvider;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, TokenProvider tokenProvider, IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenProvider = tokenProvider;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Tìm user từ username
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                // Kiểm tra mật khẩu
                var isCheckPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!isCheckPassword) return BadRequest("Invalid password");

                // Tạo token
                var token = await _tokenProvider.CreateUserTokenAsync(user);
                var refreshToken = await _tokenProvider.CreateRefreshTokenAsync();

                user.RefreshToken = refreshToken;
                user.ExpiryRefreshToken = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("JwtSettings:RefreshTokenDurationInDay"));

                await _userManager.UpdateAsync(user);
                return Ok(new TokenApiModel
                {
                    Accesstoken = token,
                    RefreshToken = refreshToken
                });
            }
            return NotFound($"User name {model.Username} is not contain");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "User registered successfully!" });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");
            var principal = _tokenProvider.GetPrincipalFromExpiredToken(tokenApiModel.Accesstoken);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null || user.RefreshToken != tokenApiModel.RefreshToken || user.ExpiryRefreshToken <= DateTime.UtcNow)
                return Unauthorized("Invalid refresh token.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var newAccessToken = await _tokenProvider.CreateUserTokenAsync(user);
            var newRefreshToken = await _tokenProvider.CreateRefreshTokenAsync();

            user.RefreshToken = newRefreshToken;
            user.ExpiryRefreshToken = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("JwtSettings:RefreshTokenDurationInDay"));


            await _userManager.UpdateAsync(user);
            return Ok(new TokenApiModel()
            {
                Accesstoken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost, Authorize]
        [Route("revoke-token")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
