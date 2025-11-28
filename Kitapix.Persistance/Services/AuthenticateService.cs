using Azure.Core;
using Kitapix.Application.Abstractions;
using Kitapix.Application.Features.LoginFeatures;
using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using Kitapix.Persistance.DbContext;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Persistance.Services
{
	public class AuthenticateService : IAuthenticateService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IJwtTokenService _jwtTokenService;

		public AuthenticateService(UserManager<AppUser> userManager, IJwtTokenService jwtTokenService)
		{
			_userManager = userManager;
			_jwtTokenService = jwtTokenService;
		}

		public async Task<LoginCommandResponse> LoginAsync(LoginCommand loginRequest)
		{
			AppUser? appUser = await _userManager.Users.Where(x => x.UserName == loginRequest.EmailorUserName || x.Email == loginRequest.EmailorUserName).FirstOrDefaultAsync();

			LoginCommandResponse loginResponse = new()
			{
				UserId = appUser.Id,
				Email = appUser.Email,
				UserName = appUser.UserName,
				Token = await _jwtTokenService.GetGenerateTokenAsync(appUser)
			};
			return loginResponse;

		}
	}
}
