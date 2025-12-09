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
	public class CreateBookCommand : IRequest<CreateBookResponse>
	{
		public required string Title { get; set; }
		public required string Description { get; set; }
		public string? CoverImageUrl { get; set; }
		public List<int> CategoryIds { get; set; } // bir kitabın birden fazla kategorisi olabilir
	}
	public class CreateBookValidator : AbstractValidator<CreateBookCommand>
	{
		public CreateBookValidator()
		{
			//RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş olamaz");
			//RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş olamaz");

		}
	}
	public class CreateBookHandler : IRequestHandler<CreateBookCommand, CreateBookResponse>
	{
		private readonly IBookRepository _bookRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IBookStatRepository _bookStatRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IImageService _imageService;

		public CreateBookHandler(IBookRepository bookRepository,
			IMapper mapper,
			IUnitOfWork unitOfWork,
			IHttpContextAccessor contextAccessor,
			IBookStatRepository bookStatRepository,
			ICategoryRepository categoryRepository,
			IImageService imageService)
		{
			_bookRepository = bookRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_contextAccessor = contextAccessor;
			_bookStatRepository = bookStatRepository;
			_categoryRepository = categoryRepository;
			_imageService = imageService;
		}

		public async Task<CreateBookResponse> Handle(CreateBookCommand request, CancellationToken cancellationToken)
		{
			var book = _mapper.Map<Book>(request);
			book.AuthorId = 1;

	
			//if (request.CoverImage != null)
			//{
			//	using var stream = request.CoverImage.OpenReadStream();
			//	string imageUrl = await _imageService.UploadImageAsync(stream, request.CoverImage.FileName, ImageType.BookCover);
			//	book.CoverImageUrl = imageUrl;
			//}

		
			book.Stats = new BookStats
			{
				LikeCount = 0,
				ViewCount = 0
			};

	
			book.BookCategories = new List<BookCategory>();
			foreach (var categoryId in request.CategoryIds)
			{
				book.BookCategories.Add(new BookCategory
				{
					Book = book,
					CategoryId = categoryId
				});
			}

			await _bookRepository.AddAsync(book);
			await _unitOfWork.SaveChangesAsync();

			return new CreateBookResponse();
		}
	}
	public class CreateBookResponse
	{
		public string Message { get; set; } = "Kitap başarıyla oluşturuldu."; 
	}
}
