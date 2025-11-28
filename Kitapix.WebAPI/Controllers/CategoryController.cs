using Kitapix.Application.Features.CategoryFeatures;
using Kitapix.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kitapix.WebAPI.Controllers
{
	public class CategoryController :BaseApiController 
	{
		public CategoryController(IMediator mediator) : base(mediator)
		{
		}

		[HttpPost("create-category")]
		public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryCommand createCategoryCommand)
		{
			CreateCategoryCommandResponse createCategoryResponse = await _mediator.Send(createCategoryCommand);
			return Ok(createCategoryResponse);
		}

		[HttpPut("update-category")]
		public async Task<IActionResult> UpdateCategory([FromForm] UpdateCategoryCommand request)
		{
			UpdateCategoryCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpDelete("delete-category")]
		public async Task<IActionResult> DeleteCategory([FromForm] DeleteCategoryCommand request)
		{
			DeleteCategoryCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("getbyid-category")]
		public async Task<IActionResult> GetCategoryById(GetByIdCategoryQuery request)
		{
			GetByIdCategoryQueryResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("getall-category")]
		public async Task<IActionResult> GetAllCategorys()
		{
			GetAllCategoryQuery request = new();
			List<GetAllCategoryQueryResponse> response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
