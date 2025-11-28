using FluentValidation;
using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Kitapix.Application.Features.AuthFeatures
{
	public class SendPasswordResetLinkCommand : IRequest<SendPasswordResetLinkResponse>
	{
		public required string Email { get; set; }
	}
	public class SendPasswordResetLinkValidator: AbstractValidator<SendPasswordResetLinkCommand>
	{
		public SendPasswordResetLinkValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("E-posta adresi boş olamaz.")
				.EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

		}
	}
	public class SendPasswordResetLinkHandler : IRequestHandler<SendPasswordResetLinkCommand, SendPasswordResetLinkResponse>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;
		public SendPasswordResetLinkHandler(UserManager<AppUser> userManager, IEmailService emailService, IConfiguration configuration)
		{
			_userManager = userManager;
			_emailService = emailService;
			_configuration = configuration;
		}
		
		public async Task<SendPasswordResetLinkResponse> Handle(SendPasswordResetLinkCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
				throw new Exception("Kullanıcı bulunamadı");
		
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
	
			var frontendUrl = _configuration["ForgotPassword"]; 
			var resetUrl = $"{frontendUrl}?email={request.Email}&token={Uri.EscapeDataString(token)}";			

			await _emailService.SendTemplatedEmailAsync(user.Email!, "Şifre Sıfırlama Talebi", "ResetLink.html", new Dictionary<string, string>
			{
				{ "ResetUrl", resetUrl   },
				{ "UserName", user.UserName! }
			});
			
			return new SendPasswordResetLinkResponse();		
		}
	}

	public class SendPasswordResetLinkResponse
	{
		public string Message { get; set; } = "Mail başarıyla gönderildi";
	}
}
