using AutoMapper;
using FluentValidation;
using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using Kitapix.Domain.Enums;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kitapix.Application.Features.BookFeatures
{
	public class UpdateBookCommand : IRequest<UpdateBookResponse>
	{
		public int Id { get; set; }
		public required string Title { get; set; }
		public required string Description { get; set; }
		public IFormFile? CoverImage { get; set; }
		public List<int> CategoryIds { get; set; } = new(); 
	}
	public class UpdateBookValidator : AbstractValidator<UpdateBookCommand>
	{
		public UpdateBookValidator() {
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id boş olamaz");
		
		}
	}
	public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, UpdateBookResponse>
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IImageService _imageService;

		public UpdateBookHandler(
			IBookRepository bookRepository,
			IMapper mapper,
			IUnitOfWork unitOfWork,
			IImageService imageService)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_imageService = imageService;
		}

		public async Task<UpdateBookResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
		{
			var existingBook = await _bookRepository.GetByIdAsync(request.Id);
			if (existingBook == null)
			{
				throw new Exception("Kitap bulunamadı.");
			}

			existingBook.Title = request.Title;
			existingBook.Description = request.Description;

			if (request.CoverImage != null)
			{
				using (var stream = request.CoverImage.OpenReadStream())
				{
					var newCoverUrl = await _imageService.UpdateImageAsync(
						existingBook.CoverImageUrl,
						stream,
						request.CoverImage.FileName,
						ImageType.BookCover);

					existingBook.CoverImageUrl = newCoverUrl;
				}
			}

			existingBook.BookCategories.Clear();
			foreach (var categoryId in request.CategoryIds)
			{
				existingBook.BookCategories.Add(new BookCategory
				{
					BookId = existingBook.Id,
					CategoryId = categoryId
				});
			}

			_bookRepository.Update(existingBook);
			await _unitOfWork.SaveChangesAsync();

			return new UpdateBookResponse();
		}
	}

	public class UpdateBookResponse
	{
		public string Message { get; set; } = "Kitap başarıyla güncellendi.";
	}
}
