using Google.Apis.Auth;
using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Claims;

namespace Kitapix.Application.Features.AuthFeatures
{
	public class GoogleLoginCommand : IRequest<GoogleLoginResponse>
	{
        public string IdToken { get; set; }

        public GoogleLoginCommand(string idToken)
        {
            this.IdToken = idToken;
        }

    }

	public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, GoogleLoginResponse>
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public GoogleLoginCommandHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            IJwtTokenService jwtTokenService , IConfiguration configuration,IEmailService emailService)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_jwtTokenService = jwtTokenService;
            _configuration = configuration;
            _emailService = emailService;
        }

		public async Task<GoogleLoginResponse> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
		{
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);
            var email = payload.Email;
            var name = payload.GivenName;
            var surname = payload.FamilyName;
            var picture = payload.Picture;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    Name = name,
                    SurName = surname,
                    UserName = email.Split('@')[0],
                    Email = email,
                    CoverImageUrl = picture,
                };

                var createUserResult = await _userManager.CreateAsync(user);
                if (!createUserResult.Succeeded)
                {
                    throw new Exception("Kullanıcı oluşturulamadı");
                }
            }
            var emailtoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var frontendUrl = _configuration["EmailConfirm"];
            var confirmationLink = $"{frontendUrl}?userId={user.Id}&token={WebUtility.UrlEncode(emailtoken)}";

            await _emailService.SendTemplatedEmailAsync(user.Email!, "E-Posta Doğrulama", "ConfirmEmail.html", new Dictionary<string, string>
            {
                { "ConfirmUrl", confirmationLink   }
            });

            
            await _signInManager.SignInAsync(user, true);

            var token = await _jwtTokenService.GetGenerateTokenAsync(user);
            GoogleLoginResponse googleLoginResponse = new()
            {
                Token = token,
            };
            return googleLoginResponse;

		}
	}

	public class GoogleLoginResponse
	{
		public required string Token { get; set; }
	}
}
