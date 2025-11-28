using Kitapix.Application.Features.BookChapterFeatures;
using Kitapix.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kitapix.WebAPI.Controllers
{
	public class BookChapterController : BaseApiController
	{
		public BookChapterController(IMediator mediator) : base(mediator)
		{
		}

		[HttpPost("create-book-chapter")]
		public async Task<IActionResult> CreateBookChapter(CreateBookChapterCommand createBookChapterCommand)
		{
			CreateBookChapterResponse createBookChapterResponse = await _mediator.Send(createBookChapterCommand);
			return Ok(createBookChapterResponse);
		}

		[HttpPut("update-book-chapter")]
		public async Task<IActionResult> UpdateBookChapter(UpdateBookChapterCommand request)
		{
			UpdateBookChapterResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpDelete("delete-book-chapter")]
		public async Task<IActionResult> DeleteBookChapter(DeleteBookChapterCommand request)
		{
			DeleteBookChapterCommandResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("get-by-book-chapter-id")]
		public async Task<IActionResult> GetBookChapterById(GetByIdBookChapterQuery request)
		{
			GetByIdBookChapterQueryResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("getall-book-chapter")]
		public async Task<IActionResult> GetAllBookChapters()
		{
			GetAllChaptersQuery request = new();
			List<GetAllChaptersQueryResponse> response = await _mediator.Send(request);
			return Ok(response);
		}		
	}
}
