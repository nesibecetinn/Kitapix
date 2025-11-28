using AutoMapper;
using Kitapix.Application.Services;
using Kitapix.Domain.Dtos;
using Kitapix.Domain.Repositories;
using MediatR;

namespace Kitapix.Application.Features.BookFeatures
{
	public class GetAllBookByCreateDateQuery : IRequestWithoutValidator<List<GetAllBookByCreateDateQueryResponse>>
	{
	}
	public class GetAllBookByCreateDateQueryHandler : IRequestHandler<GetAllBookByCreateDateQuery, List<GetAllBookByCreateDateQueryResponse>>
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;
		private readonly IUserBookCategoryService _userBookCategoryService;
		public GetAllBookByCreateDateQueryHandler(IBookRepository bookRepository, IMapper mapper, IUserBookCategoryService userBookCategoryService)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
			_userBookCategoryService = userBookCategoryService;
		}

		public async Task<List<GetAllBookByCreateDateQueryResponse>> Handle(GetAllBookByCreateDateQuery request, CancellationToken cancellationToken)
		{
			var categoryIds = await _userBookCategoryService.GetUserCategoryIdsAsync(); //kullanıcının seçtiği kitap kategori listesi

			var books = await _bookRepository
				.GetBookByCreateDate(categoryIds); // kullanıcının seçtiği kitap kategorisine ve son ekleme tarihine göre kitap listesi 

			var response = _mapper.Map<List<GetAllBookByCreateDateQueryResponse>>(books);
			return response;
		}
	}

	public class GetAllBookByCreateDateQueryResponse
	{
		public BookDto BookDto { get; set; }
	}
}
