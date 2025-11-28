using AutoMapper;
using FluentValidation;
using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using Kitapix.Domain.Enums;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kitapix.Application.Features.CategoryFeatures
{
	public class CreateCategoryCommand : IRequest<CreateCategoryCommandResponse>
	{	
		public string Name { get; set; }
		public IFormFile? Url { get; set; }
	}

	public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
	{
		public CreateCategoryCommandValidator() {
			RuleFor(x => x.Name).NotEmpty().WithMessage("Ad alanı boş olamaz");
			RuleFor(x => x.Url).NotEmpty().WithMessage("Url alanı boş olamaz");

		}
	}
	public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IImageService _imageService; // 📷 Resim servisi eklendi

		public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_imageService = imageService;
		}

		public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
		{
			var category = await _categoryRepository.GetCategoryByName(request.Name);
			if (category != null)
			{
				throw new Exception("Bu ada sahip bir kategori zaten var.");
			}

			category = _mapper.Map<Category>(request);

			
			if (request.Url != null)
			{
				using var stream = request.Url.OpenReadStream();
				var imageUrl = await _imageService.UploadImageAsync(stream, request.Url.FileName, ImageType.CategoryImage);
				category.Url = imageUrl;
			}

			await _categoryRepository.AddAsync(category);
			await _unitOfWork.SaveChangesAsync();

			return new CreateCategoryCommandResponse();
		}
	}

	public class CreateCategoryCommandResponse
	{
		public string Message { get; set; } = "Kategori başarıyla eklendi";
 	}
}
