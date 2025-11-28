using Kitapix.Application.Features.BookFeatures;
using Kitapix.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kitapix.WebAPI.Controllers
{
	public class BookController : BaseApiController
	{
		public BookController(IMediator mediator) : base(mediator)
		{
		}

		[HttpPost("create-book")]
		public async Task<IActionResult> CreateBook([FromForm] CreateBookCommand request)
		{
			CreateBookResponse createBookResponse = await _mediator.Send(request);
			return Ok(createBookResponse);
		}

		[HttpPut("update-book")]
		public async Task<IActionResult> UpdateBook([FromForm] UpdateBookCommand request)
		{
			UpdateBookResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpDelete("delete-book")]
		public async Task<IActionResult> DeleteBook([FromForm] DeleteBookCommand request)
		{
			DeleteBookResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpPost("get-book-byid")]
		public async Task<IActionResult> GetBookById(GetBookByIdQuery request)
		{

			GetBookByIdResponse response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("getall-book")]
		public async Task<IActionResult> GetAllBooks()
		{
			GetAllBookQuery request = new();
			List<GetAllBookResponse> response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("getall-book-with-chapters")]
		public async Task<IActionResult> GetAllBookWithChapters()
		{
			GetAllBookWithChaptersQuery request = new();
			List<GetAllBookWithChaptersResponse> response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpGet("getall-book-by-viewcount")]
		public async Task<IActionResult> GetBookByViewCount()
		{
			GetBookByViewCountQuery request = new();
			List<GetBookByViewCountQueryResponse> response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpGet("getall-book-by-createdate")]
		public async Task<IActionResult> GetBookByCreateDate()
		{
			GetAllBookByCreateDateQuery request = new();
			List<GetAllBookByCreateDateQueryResponse> response = await _mediator.Send(request);
			return Ok(response);
		}
		[HttpGet("getall-book-all-info")]
		public async Task<IActionResult> GetAllBookAllInfo()
		{
			GetAllBookWithAllInfoQuery request = new();
			List<GetAllBookWithAllInfoQueryResponse> response = await _mediator.Send(request);
			return Ok(response);
		}

		[HttpGet("getall-book-by-userId")]
		public async Task<IActionResult> GetBookListByUserId()
		{
			GetBookListByUserIdQuery request = new();
			List<GetBookListByUserIdQueryResponse> response = await _mediator.Send(request);
			return Ok(response);
		}
	}
}
