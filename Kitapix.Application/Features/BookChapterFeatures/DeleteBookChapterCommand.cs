using AutoMapper;
using FluentValidation;
using Kitapix.Domain.Repositories.MongoDbRepositoreis;
using Kitapix.Domain.UnitOfWork;
using MediatR;

namespace Kitapix.Application.Features.BookChapterFeatures
{
	public class DeleteBookChapterCommand : IRequest<DeleteBookChapterCommandResponse>
	{
		public string Id { get; set; }
	}
	public class DeleteBookChapterValidator : AbstractValidator<DeleteBookChapterCommand>
	{
		public DeleteBookChapterValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");
		}
	}
	public class DeleteBookChapterCommandHandler : IRequestHandler<DeleteBookChapterCommand, DeleteBookChapterCommandResponse>
	{
		private readonly IBookChapterRepository _bookChapterRepository;
		private readonly Domain.Repositories.IBookChapterRepository _bookChapterSqlRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public DeleteBookChapterCommandHandler(IBookChapterRepository bookChapterRepository,
			Domain.Repositories.IBookChapterRepository bookChapterSqlRepository,
			IMapper mapper,
			IUnitOfWork unitOfWork)
		{
			_bookChapterSqlRepository = bookChapterSqlRepository;
			_bookChapterRepository = bookChapterRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<DeleteBookChapterCommandResponse> Handle(DeleteBookChapterCommand request, CancellationToken cancellationToken)
		{
			var existingChapter = await _bookChapterRepository.GetByIdAsync(request.Id);
			if (existingChapter == null)
			{
				throw new Exception("Kitap bölümü bulunamadı.");
			}
			await _bookChapterRepository.DeleteAsync(request.Id);
			var result = await _bookChapterSqlRepository.GetBookChapterByMongoDbIdAsync(request.Id);
			if (result == null)
			{
				throw new Exception("Kitap bölümü bulunamadı.");
			}
			await _bookChapterSqlRepository.DeleteAsync(result!.Id);
			await _unitOfWork.SaveChangesAsync();
			return new DeleteBookChapterCommandResponse();
		}
	}

	public class DeleteBookChapterCommandResponse
	{
		public string Message { get; set; } = "Bölüm başarıyla silindi.";
	}
}
