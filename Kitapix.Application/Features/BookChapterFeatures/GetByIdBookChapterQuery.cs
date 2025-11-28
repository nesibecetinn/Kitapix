using AutoMapper;
using FluentValidation;
using Kitapix.Domain.Repositories.MongoDbRepositoreis;
using MediatR;

namespace Kitapix.Application.Features.BookChapterFeatures
{
	public class GetByIdBookChapterQuery : IRequest<GetByIdBookChapterQueryResponse>
	{
		public required string Id { get; set; }
	}
	public class GetByIdBookChapterQueryValidator : AbstractValidator<GetByIdBookChapterQuery>
	{
		public GetByIdBookChapterQueryValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");
		}
	}
	public class GetByIdChapterQueryHandler : IRequestHandler<GetByIdBookChapterQuery, GetByIdBookChapterQueryResponse>
	{
		private readonly IBookChapterRepository _bookRepository;
		private readonly IMapper _mapper;

		public GetByIdChapterQueryHandler(IBookChapterRepository bookRepository, IMapper mapper)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
		}
		public async Task<GetByIdBookChapterQueryResponse> Handle(GetByIdBookChapterQuery request, CancellationToken cancellationToken)
		{
			var existingChapter = await _bookRepository.GetByIdAsync(request.Id);
			if (existingChapter == null)
			{
				throw new Exception("Kitap bulunamadı.");
			}
			GetByIdBookChapterQueryResponse response = _mapper.Map<GetByIdBookChapterQueryResponse>(existingChapter);
			return response;

		}
	}

	public class GetByIdBookChapterQueryResponse
	{
		public string Id { get; set; }
		public int BookId { get; set; }
		public required string Title { get; set; }
		public required string Content { get; set; }
	}
}
