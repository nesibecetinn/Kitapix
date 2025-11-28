using Kitapix.Application.Features.UserFeatures;
using Kitapix.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kitapix.WebAPI.Controllers
{
	public class UserController : BaseApiController
	{
		public UserController(IMediator mediator) : base(mediator)
		{
		}

		[HttpPost("user-book-categoryIds")]
		public async Task<IActionResult> CreateUserBookCategoryIds(CreateUserBookCategoriesCommand request)
		{
			CreateUserBookCategoriesCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
