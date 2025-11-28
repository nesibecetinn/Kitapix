using FluentValidation;
using Kitapix.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kitapix.Application.Features.AuthFeatures
{
	public class PasswordResetCommand : IRequest<PasswordResetResponse>
	{
		public required string Email { get; set; }
		public required string Token { get; set; }
		public required string NewPassword { get; set; }
		public required string ConfirmPassword { get; set; }

	}
	public class PasswordResetCommandValidator : AbstractValidator<PasswordResetCommand>
	{
		public PasswordResetCommandValidator()
		{
			RuleFor(x => x.Email)
		   .NotEmpty().WithMessage("E-posta adresi boş olamaz.")
		   .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

			RuleFor(x => x.NewPassword)
				.NotEmpty().WithMessage("Şifre boş olamaz.")
				.MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
				.Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
				.Matches(@"[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
				.Matches(@"\d").WithMessage("Şifre en az bir rakam içermelidir.")
				.Matches(@"[\W_]").WithMessage("Şifre en az bir özel karakter içermelidir.");

			RuleFor(x => x.ConfirmPassword)
				.NotEmpty().WithMessage("Şifre tekrarı boş olamaz.")
				.Equal(x => x.NewPassword).WithMessage("Şifreler eşleşmiyor.");

		}
	}
	public class PasswordResetHandler : IRequestHandler<PasswordResetCommand, PasswordResetResponse>
	{
		private readonly UserManager<AppUser> _userManager;

		public PasswordResetHandler(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public async Task<PasswordResetResponse> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user == null)
				throw new Exception("Kullanıcı bulunamadı");


			var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

			return new PasswordResetResponse();
			
		}
	}

	public class PasswordResetResponse
	{
		public string Message { get; set; } = "Şifre başarıyla değiştirildi";
	}
}
