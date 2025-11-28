using AutoMapper;
using Kitapix.Domain.Entities.MongoEntities;
using Kitapix.Domain.Repositories;
using MediatR;

namespace Kitapix.Application.Features.BookFeatures
{
	public class GetAllBookWithChaptersQuery : IRequestWithoutValidator<List<GetAllBookWithChaptersResponse>>
	{
	}
	public class GetAllBookWithChaptersQueryHandler : IRequestHandler<GetAllBookWithChaptersQuery, List<GetAllBookWithChaptersResponse>>
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;
		private readonly Domain.Repositories.MongoDbRepositoreis.IBookChapterRepository _chapterRepository;
		public GetAllBookWithChaptersQueryHandler(IBookRepository bookRepository, IMapper mapper, Domain.Repositories.MongoDbRepositoreis.IBookChapterRepository chapterRepository)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
			_chapterRepository = chapterRepository;
		}
		public async Task<List<GetAllBookWithChaptersResponse>> Handle(GetAllBookWithChaptersQuery request, CancellationToken cancellationToken)
		{
			var books = await _bookRepository.GetAllAsync();

			List<GetAllBookWithChaptersResponse> response = _mapper.Map<List<GetAllBookWithChaptersResponse>>(books);

			foreach (var bookResponse in response)
			{
				var chapters = await _chapterRepository.GetChaptersByBookIdAsync(bookResponse.Id);
				bookResponse.Chapters = chapters.ToList(); 
			}

			return response;		
		}
	}

	public class GetAllBookWithChaptersResponse
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public List<BookChapter> Chapters { get; set; }
	}
}
