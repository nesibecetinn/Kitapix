using AutoMapper;
using Kitapix.Application.Services;
using Kitapix.Domain.Repositories;
using MediatR;

namespace Kitapix.Application.Features.BookFeatures
{
	public class GetBookListByUserIdQuery : IRequestWithoutValidator<List<GetBookListByUserIdQueryResponse>>
	{

	}
	public class GetBookListByUserIdQueryHandler : IRequestHandler<GetBookListByUserIdQuery, List<GetBookListByUserIdQueryResponse>>
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;
		private readonly IUserBookCategoryService _userBookCategoryService;
		public GetBookListByUserIdQueryHandler(IBookRepository bookRepository, IMapper mapper, IUserBookCategoryService userBookCategoryService)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
			_userBookCategoryService = userBookCategoryService;
		}
		public async Task<List<GetBookListByUserIdQueryResponse>> Handle(GetBookListByUserIdQuery request, CancellationToken cancellationToken)
		{
			//var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
			//if (userIdClaim == null)
			//	throw new Exception("Kullanıcı doğrulanamadı");

			var userId = 1;

			var books = await _bookRepository.GetBookListByUserId(userId);

			var response = _mapper.Map<List<GetBookListByUserIdQueryResponse>>(books);
			return response;
		}
	}

	public class GetBookListByUserIdQueryResponse
	{
	}
}
