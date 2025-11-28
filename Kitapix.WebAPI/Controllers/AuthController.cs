
using Kitapix.Application.Features.AuthFeatures;
using Kitapix.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kitapix.WebAPI.Controllers
{
	public class AuthController : BaseApiController
	{
		public AuthController(IMediator mediator) : base(mediator)
		{
		}

		[HttpPost("login")]
		public async Task<IActionResult> LoginAsync(LoginCommand loginRequest)
		{
			LoginCommandResponse loginCommandResponse = await _mediator.Send(loginRequest);	
			return Ok(loginCommandResponse);
		}
		[HttpPost("register")]
		public async Task<IActionResult> RegisterAsync(RegisterCommand registerRequest)
		{
			RegisterCommandResponse registerCommandResponse = await _mediator.Send(registerRequest);
			return Ok(registerCommandResponse);
		}
		[HttpPost("google-login")]
		public async Task<IActionResult> GoogleLogin([FromQuery] string idToken)
		{
			var result = await _mediator.Send(new GoogleLoginCommand(idToken));
			return Ok(result); 
		
		}
		[HttpPost("confirm-email")]
		public async Task<IActionResult> ConfirmEmail([FromQuery] int userId, [FromQuery] string token)
		{
			EmailConfirmCommand emailConfirmCommand = new()
			{
				Token = token,
				UserId = userId
			};
			EmailConfirmResponse emailConfirmResponse = await _mediator.Send(emailConfirmCommand);
			return Ok(emailConfirmResponse);
		}
		[HttpPost("request-password-reset")]
		public async Task<IActionResult> RequestPasswordReset([FromBody] SendPasswordResetLinkCommand sendPasswordResetLinkCommand)
		{
			var result = await _mediator.Send(sendPasswordResetLinkCommand);
			
			return Ok(result);
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] PasswordResetCommand command)
		{
			var result = await _mediator.Send(command);		
			return Ok(result);
		}
	}
}
