using AutoMapper;
using Kitapix.Domain.Repositories;
using MediatR;

namespace Kitapix.Application.Features.BookFeatures
{
	public class GetAllBookQuery : IRequestWithoutValidator<List<GetAllBookResponse>>
	{
	}
	public class GetAllBookQueryHandler : IRequestHandler<GetAllBookQuery, List<GetAllBookResponse>>
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;

		public GetAllBookQueryHandler(IBookRepository bookRepository, IMapper mapper)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;	
		}
		public async Task<List<GetAllBookResponse>> Handle(GetAllBookQuery request, CancellationToken cancellationToken)
		{
			
			var books = await _bookRepository.GetAllAsync();

			List<GetAllBookResponse> response = _mapper.Map<List<GetAllBookResponse>>(books);
		
			return response;	
		}
	}

	public class GetAllBookResponse
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	

	}
}
