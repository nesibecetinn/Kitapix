using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Kitapix.Infrastructure.Authentication.Jwt
{
	public class JwtTokenService : IJwtTokenService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly JwtSettings _jwtSettings;
		public JwtTokenService(UserManager<AppUser> userManager, IOptions<JwtSettings> jwtOptions)
		{
			_userManager = userManager;
			_jwtSettings = jwtOptions.Value;
		}
		public async Task<string> GetGenerateTokenAsync(AppUser appUser)
		{

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()), 
				new Claim(ClaimTypes.Name, appUser.UserName!),
				new Claim(ClaimTypes.Email, appUser.Email!),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};
			var expires = DateTime.Now.AddDays(1);
			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: expires,
				signingCredentials: creds
			);

			string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

			appUser.RefreshToken = refreshToken;
			appUser.RefreshTokenExpires = expires.AddDays(1);
			await _userManager.UpdateAsync(appUser);


			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
