using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Kitapix.WebAPI.Abstractions
{
	[ApiController]
	[Route("api/[controller]")]
	public abstract class BaseApiController :ControllerBase	
	{
		protected readonly IMediator _mediator;

		protected BaseApiController(IMediator mediator)
		{
			_mediator = mediator;
		}
	}
}
