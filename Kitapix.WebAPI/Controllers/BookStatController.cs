using Kitapix.Application.Features.BookStatFeatures;
using Kitapix.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kitapix.WebAPI.Controllers
{
	public class BookStatController : BaseApiController
	{
		public BookStatController(IMediator mediator) : base(mediator)
		{
		}

		[HttpPost("like")]
		public async Task<IActionResult> IncreaseLikeCount(IncreaseLikeCountCommand request)
		{
			IncreaseLikeCountCommandReponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("unlike")]
		public async Task<IActionResult> IncreaseViewCount(IncreaseViewCountCommand request)
		{
			IncreaseViewCountCommandReponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("ViewCount")]
		public async Task<IActionResult> DecreaseLikeCount(DecreaseLikeCountCommand request)
		{
			DecreaseLikeCountCommandReponse response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
