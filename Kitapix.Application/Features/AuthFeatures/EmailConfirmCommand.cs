using FluentValidation;
using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net;
 
namespace Kitapix.Application.Features.AuthFeatures
{
	public class EmailConfirmCommand : IRequest<EmailConfirmResponse>
	{
		public required int UserId { get; set; }
		public required string Token { get; set; }
	}
	public class EmailConfirmCommandValidator : AbstractValidator<EmailConfirmCommand>
	{
		public EmailConfirmCommandValidator()
		{
			RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId zorunlu.");
			RuleFor(x => x.Token).NotEmpty().WithMessage("Token zorunlu.");
		}
	}
	public class EmailConfirmHandler : IRequestHandler<EmailConfirmCommand, EmailConfirmResponse>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;

		public EmailConfirmHandler(UserManager<AppUser> userManager, IEmailService emailService, IConfiguration configuration)
		{
			_userManager = userManager;
			_emailService = emailService;
			_configuration = configuration;
		}

		public async Task<EmailConfirmResponse> Handle(EmailConfirmCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.UserId.ToString());
			if (user == null)
				throw new KeyNotFoundException("Kullanıcı bulunamadı");

			var result = await _userManager.ConfirmEmailAsync(user, request.Token);
			if (result.Succeeded)
			{
				var frontendUrl = _configuration["ClientAdminUrl"];
				var confirmationLink = $"{frontendUrl}";

				await _emailService.SendTemplatedEmailAsync(user.Email!, "Hoşgeldiniz", "Welcome.html", new Dictionary<string, string>
				{
					{ "LoginUrl", confirmationLink   },
					{ "UserName", user.UserName! },

				});
				return new EmailConfirmResponse();
			}

			throw new KeyNotFoundException("Email doğrulanamadı");
		}
	}

	public class EmailConfirmResponse
	{
		public string Message { get; set; } = "Email başarıyla doğrulandı";
	}
}
