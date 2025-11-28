using AutoMapper;
using Kitapix.Application.Services;
using Kitapix.Domain.Dtos;
using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using MediatR;

namespace Kitapix.Application.Features.BookFeatures
{
	public class GetBookByViewCountQuery : IRequestWithoutValidator<List<GetBookByViewCountQueryResponse>>
	{
	}
	public class GetBookByViewCountQueryHandler : IRequestHandler<GetBookByViewCountQuery, List<GetBookByViewCountQueryResponse>>
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;
		private readonly IUserBookCategoryService _userBookCategoryService;
		public GetBookByViewCountQueryHandler(IBookRepository bookRepository, IMapper mapper, IUserBookCategoryService userBookCategoryService)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
			_userBookCategoryService = userBookCategoryService;
		}

		public async Task<List<GetBookByViewCountQueryResponse>> Handle(GetBookByViewCountQuery request, CancellationToken cancellationToken)
		{
			var categoryIds = await _userBookCategoryService.GetUserCategoryIdsAsync();
			var books = await _bookRepository.GetBookByViewCount(categoryIds);
			
			List<GetBookByViewCountQueryResponse> sortedBook =  _mapper.Map<List<GetBookByViewCountQueryResponse>>(books);
			return sortedBook;
		}
	}
	public class GetBookByViewCountQueryResponse
	{
		public BookDto BookDto { get; set; }

	}
}
