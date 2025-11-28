using AutoMapper;
using FluentValidation;
using Kitapix.Domain.Repositories.MongoDbRepositoreis;
using Kitapix.Domain.UnitOfWork;
using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kitapix.Application.Features.BookChapterFeatures
{
	public class UpdateBookChapterCommand : IRequest<UpdateBookChapterResponse>
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public required string Id { get; set; } = ObjectId.GenerateNewId().ToString();
		public required string Title { get; set; }
		public required string Content { get; set; }

	}
	public class UpdateBookChapterValidator : AbstractValidator<UpdateBookChapterCommand>
	{
		public UpdateBookChapterValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");
			RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz");
			RuleFor(x => x.Content).NotEmpty().WithMessage("İçerik boş olamaz");
		}
	}
	public class UpdateBookChapterHandler : IRequestHandler<UpdateBookChapterCommand, UpdateBookChapterResponse>
	{
		private readonly IBookChapterRepository _bookChapterRepository;
		private IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public UpdateBookChapterHandler(IBookChapterRepository bookChapterRepository, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_bookChapterRepository = bookChapterRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<UpdateBookChapterResponse> Handle(UpdateBookChapterCommand request, CancellationToken cancellationToken)
		{
			var bookChapter = await _bookChapterRepository.GetByIdAsync(request.Id);
			if (bookChapter == null)
			{
				throw new Exception("Bölüm bulunamadı.");
			}
			bookChapter.UpdatedDate = DateTime.Now;
			_mapper.Map(request, bookChapter);
			_bookChapterRepository.Update(bookChapter);
			await _unitOfWork.SaveChangesAsync();
			return new UpdateBookChapterResponse();
		}
	}
	public class UpdateBookChapterResponse
	{
		public string Message { get; set; } = "Bölüm başarıyla güncellendi.";
	}
}
