using AutoMapper;
using Kitapix.Domain.Repositories.MongoDbRepositoreis;
using MediatR;

namespace Kitapix.Application.Features.BookChapterFeatures
{
	public class GetAllChaptersQuery : IRequestWithoutValidator<List<GetAllChaptersQueryResponse>>
	{
	}
	public class GetAllChaptersQueryHandler : IRequestHandler<GetAllChaptersQuery, List<GetAllChaptersQueryResponse>>
	{
		private readonly IBookChapterRepository _bookChapterRepository;
		private readonly IMapper _mapper;
		public GetAllChaptersQueryHandler(IBookChapterRepository bookChapterRepository, IMapper mapper)
		{
			_bookChapterRepository = bookChapterRepository;
			_mapper = mapper;
		}

		public async Task<List<GetAllChaptersQueryResponse>> Handle(GetAllChaptersQuery request, CancellationToken cancellationToken)
		{
			var response = await _bookChapterRepository.GetAllAsync();
			List<GetAllChaptersQueryResponse> chapters = _mapper.Map<List<GetAllChaptersQueryResponse>>(response);

			return chapters;
		}
	}

	public class GetAllChaptersQueryResponse
	{
		public string Id { get; set; }
		public int BookId { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
	}
}
