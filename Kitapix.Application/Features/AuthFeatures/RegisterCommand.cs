using AutoMapper;
using FluentValidation;
using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Application.Features.AuthFeatures
{
	public class RegisterCommand : IRequest<RegisterCommandResponse>
	{
		public required string Name { get; set; }
		public required string SurName { get; set; }
		public required string UserName { get; set; }
		public required string Email { get; set; }
		public required string PasswordHash { get; set; }
		public required string PhoneNumber { get; set; }


	}
	public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
	{
		public RegisterCommandValidator()
		{
			RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Ad alanı boş olamaz.")
			.MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir.");

			RuleFor(x => x.SurName)
				.NotEmpty().WithMessage("Soyad alanı boş olamaz.")
				.MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir.");

			RuleFor(x => x.UserName)
				.NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
				.MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.")
				.MaximumLength(20).WithMessage("Kullanıcı adı en fazla 20 karakter olabilir.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("E-posta adresi boş olamaz.")
				.EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

			RuleFor(x => x.PasswordHash)
				.NotEmpty().WithMessage("Şifre boş olamaz.")
				.MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
				.Matches(@"[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
				.Matches(@"[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
				.Matches(@"\d").WithMessage("Şifre en az bir rakam içermelidir.")
				.Matches(@"[\W]").WithMessage("Şifre en az bir özel karakter içermelidir.");

			RuleFor(x => x.PhoneNumber)
				.NotEmpty().WithMessage("Telefon numarası boş olamaz.")
				.Matches(@"^\+?[1-9][0-9]{7,14}$").WithMessage("Geçerli bir telefon numarası giriniz.");
		}
	}
	public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponse>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;
		private readonly IEmailService _emailService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IConfiguration _configuration;
		public RegisterCommandHandler(UserManager<AppUser> userManager, 
			IMapper mapper, 
			IEmailService emailService, 
			IHttpContextAccessor httpContextAccessor,
			IConfiguration configuration)
		{
			_userManager = userManager;
			_mapper = mapper;
			_emailService = emailService;
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
		}

		public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
		{
			AppUser? appUser = await _userManager.Users.Where(x => x.UserName == request.UserName || x.Email == request.Email).FirstOrDefaultAsync();
			if(appUser != null)
			{
				throw new KeyNotFoundException("Bu kullanıcı adı daha önce kayıt olmuş");
			}

			// Yeni kullanıcı oluştur
			var newUser = _mapper.Map<AppUser>(request);

			// Kullanıcıyı kaydet ve şifreyi ayarla
			var result = await _userManager.CreateAsync(newUser, request.PasswordHash);

			if (!result.Succeeded)
			{
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				throw new KeyNotFoundException($"Kullanıcı kaydedilemedi: {errors}");
			}

			var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

			var frontendUrl = _configuration["EmailConfirm"];
			var confirmationLink = $"{frontendUrl}?userId={newUser.Id}&token={WebUtility.UrlEncode(token)}";

			await _emailService.SendTemplatedEmailAsync(newUser.Email!, "E-Posta Doğrulama" , "ConfirmEmail.html", new Dictionary<string, string>
			{
				{ "ConfirmUrl", confirmationLink   }
			} );
			
			return new RegisterCommandResponse();
		}
	}

	public class RegisterCommandResponse
	{
		public string Message { get; set; } = "Kayıt başarıla oluşturuldu.";
	}
}
