using AutoMapper;
using FluentValidation;
using Kitapix.Domain.Entities.MongoEntities;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.Repositories.MongoDbRepositoreis;
using MediatR;

namespace Kitapix.Application.Features.BookFeatures
{
	public class GetBookByIdQuery : IRequest<GetBookByIdResponse>
	{
		public int Id { get; set; }
	}
	public class GetBookByIdValidator : AbstractValidator<GetBookByIdQuery>
	{
		public GetBookByIdValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");
		
		}
	}
	public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, GetBookByIdResponse>
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;
		private readonly Domain.Repositories.MongoDbRepositoreis.IBookChapterRepository _chapterRepository;
		public GetBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper, Domain.Repositories.MongoDbRepositoreis.IBookChapterRepository chapterRepository)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
			_chapterRepository = chapterRepository;
		}

		public async Task<GetBookByIdResponse> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
		{
			var existingBook = await _bookRepository.GetByIdAsync(request.Id);
			if (existingBook == null)
			{
				throw new Exception("Kitap bulunamadı.");
			}

			var chapters = await _chapterRepository.GetChaptersByBookIdAsync(request.Id);

		
			GetBookByIdResponse response = _mapper.Map<GetBookByIdResponse>(existingBook);
			response.Chapters = chapters; 

			return response;
		}
	}

	public class GetBookByIdResponse
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public List<BookChapter> Chapters { get; set; }
	}
}
