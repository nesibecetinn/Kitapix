using AutoMapper;
using FluentValidation;
using Kitapix.Application.Services;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using MediatR;
using static Kitapix.Application.Features.BookFeatures.DeleteBookCommandHandler;

namespace Kitapix.Application.Features.BookFeatures
{
	public class DeleteBookCommand : IRequest<DeleteBookResponse>
	{
		public int Id { get; set; }
	}
	public class DeleteBookValidator : AbstractValidator<DeleteBookCommand>
	{
		public DeleteBookValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("Kitap boş olamaz");
		}
	}
	public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, DeleteBookResponse>
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IImageService _imageService; // ImageService'i ekledik

		public DeleteBookCommandHandler(IBookRepository bookRepository, IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_imageService = imageService;
		}

		public async Task<DeleteBookResponse> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
		{
			var existingBook = await _bookRepository.GetByIdAsync(request.Id);
			if (existingBook == null)
			{
				throw new Exception("Kitap bulunamadı.");
			}

			if (!string.IsNullOrEmpty(existingBook.CoverImageUrl))
			{
				await _imageService.DeleteImageAsync(existingBook.CoverImageUrl);
			}

			await _bookRepository.DeleteAsync(request.Id);
			await _unitOfWork.SaveChangesAsync();

			return new DeleteBookResponse();
		}
	}
	public class DeleteBookResponse
	{
		public string Message { get; set; } = "Başarıyla silindi";
	}
}
