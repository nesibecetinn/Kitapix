using AutoMapper;
using FluentValidation;
using Kitapix.Domain.Entities.MongoEntities;
using Kitapix.Domain.Repositories.MongoDbRepositoreis;
using Kitapix.Domain.UnitOfWork;
using MediatR;


namespace Kitapix.Application.Features.BookChapterFeatures
{
	public class CreateBookChapterCommand : IRequest<CreateBookChapterResponse>
	{
		public int BookId { get; set; }
		public required string Title { get; set; }
		public required string Content { get; set; }

	}
	public class CreateBookChapterValidator : AbstractValidator<CreateBookChapterCommand>
	{
		public CreateBookChapterValidator()
		{
			RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz");
			RuleFor(x => x.Content).NotEmpty().WithMessage("İçerik boş olamaz");
			RuleFor(x => x.BookId).NotEmpty().WithMessage("Kitap boş olamaz");

		}
	}
	public class CreateBookChapterHandler : IRequestHandler<CreateBookChapterCommand, CreateBookChapterResponse>
	{
		private readonly IBookChapterRepository _bookChapterRepository;
		private readonly Domain.Repositories.IBookChapterRepository _bookChapterSqlRepository;
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;
		public CreateBookChapterHandler(IBookChapterRepository bookChapterRepository,
			Domain.Repositories.IBookChapterRepository bookChapterSqlRepository,
			IMapper mapper,
			IUnitOfWork unitOfWork)
		{
			_bookChapterSqlRepository = bookChapterSqlRepository;
			_bookChapterRepository = bookChapterRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<CreateBookChapterResponse> Handle(CreateBookChapterCommand request, CancellationToken cancellationToken)
		{


			var bookchapter = _mapper.Map<BookChapter>(request);
			bookchapter.CreatedDate = DateTime.Now;
			bookchapter.UpdatedDate = DateTime.Now;
			await _bookChapterRepository.AddAsync(bookchapter);
			var bookChapterSql = new Domain.Entities.BookChapter
			{
				MongoDbId = bookchapter.Id.ToString(), // MongoDB'deki ID
				BookId = request.BookId
			};
			await _bookChapterSqlRepository.AddAsync(bookChapterSql);
			await _unitOfWork.SaveChangesAsync();
			return new CreateBookChapterResponse();
		}
	}

	public class CreateBookChapterResponse
	{
		public string Message { get; set; } = "Bölüm başarıyla kaydedildi";
	}
}
