using AutoMapper;
using Kitapix.Application.Services;
using Kitapix.Domain.Dtos;
using Kitapix.Domain.Repositories;
using MediatR;

namespace Kitapix.Application.Features.BookFeatures
{
	public class GetAllBookWithAllInfoQuery : IRequestWithoutValidator<List<GetAllBookWithAllInfoQueryResponse>>
	{
	}
	public class GetAllBookWithAllInfoQueryHandler : IRequestHandler<GetAllBookWithAllInfoQuery, List<GetAllBookWithAllInfoQueryResponse>>
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;
		private readonly IUserBookCategoryService _userBookCategoryService;
		public GetAllBookWithAllInfoQueryHandler(IBookRepository bookRepository, IMapper mapper, IUserBookCategoryService userBookCategoryService)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
			_userBookCategoryService = userBookCategoryService;
		}

		public async Task<List<GetAllBookWithAllInfoQueryResponse>> Handle(GetAllBookWithAllInfoQuery request, CancellationToken cancellationToken)
		{
			var categoryIds = await _userBookCategoryService.GetUserCategoryIdsAsync(); //kullanıcının seçtiği kitap kategori listesi

			var books = await _bookRepository
				.GetAllWithStatsAndAuthorAsync(categoryIds); // kullanıcının seçtiği kitap kategorisine ve beğeni,yorum ve yazar bilgileriyle kitap listesi  

			var response = _mapper.Map<List<GetAllBookWithAllInfoQueryResponse>>(books);
			return response;
		}
	}

	public class GetAllBookWithAllInfoQueryResponse
	{
		public BookDto BookDto { get; set; }
	}
}
