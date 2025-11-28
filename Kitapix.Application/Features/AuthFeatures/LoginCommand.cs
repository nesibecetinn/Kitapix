using FluentValidation;
using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Kitapix.Application.Features.AuthFeatures
{
	public class LoginCommand :IRequest<LoginCommandResponse>
	{
		public required string EmailorUserName { get; set; }
		public required string Password { get; set; }
	}
	public class LoginCommandValidator : AbstractValidator<LoginCommand>
	{
		public LoginCommandValidator()
		{
			RuleFor(x => x.EmailorUserName)
				.NotEmpty().WithMessage("Kullanıcı adı / Email boş olamaz.")
				.MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.");


			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Şifre boş olamaz.")
				.MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
				
		}
	}
	public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
	{
		
		private readonly UserManager<AppUser> _userManager;
		private readonly IJwtTokenService _jwtTokenService;
		public LoginCommandHandler( UserManager<AppUser> userManager, IJwtTokenService jwtTokenService)
		{
			_userManager = userManager;
			_jwtTokenService = jwtTokenService;
		}

		public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
		{
			AppUser? appUser = await _userManager.Users.Where(x => x.UserName == request.EmailorUserName || x.Email == request.EmailorUserName).FirstOrDefaultAsync();
			if (appUser == null)
				throw new KeyNotFoundException("Kullanıcı bulunamadı");
			
			var checkUser = await _userManager.CheckPasswordAsync(appUser, request.Password);
			
			if (!checkUser)			
				throw new KeyNotFoundException("Şifre hatalı.");
			
			if (!appUser.EmailConfirmed)
				throw new KeyNotFoundException("Lütfen önce e-posta adresinizi doğrulayın!");

			LoginCommandResponse loginResponse = new()
			{
				UserId = appUser.Id,
				Email = appUser.Email!,
				UserName = appUser.UserName!,
				Token = await _jwtTokenService.GetGenerateTokenAsync(appUser)
			};
			return loginResponse;
		
		}
	}
	public class LoginCommandResponse 
	{
		public int UserId { get; set; }
		public required string Email { get; set; }
		public required string UserName { get; set; }
		public required string Token { get; set; }
	
	}

}
