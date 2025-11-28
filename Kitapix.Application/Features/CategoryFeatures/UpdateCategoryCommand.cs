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
	public class UpdateCategoryCommand : IRequest<UpdateCategoryCommandResponse>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public IFormFile? Image { get; set; } // <<< Yeni ekledik
	}

	public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
	{
		public UpdateCategoryCommandValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id alanı boş olamaz");
			RuleFor(x => x.Name).NotEmpty().WithMessage("Ad alanı boş olamaz");
			RuleFor(x => x.Url).NotEmpty().WithMessage("Url alanı boş olamaz");
		}
	}

	public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryCommandResponse>
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IImageService _imageService; // <<< Resim işlemleri için

		public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork unitOfWork, IImageService imageService)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_imageService = imageService;
		}

		public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
		{
			var existingCategory = await _categoryRepository.GetByIdAsync(request.Id);
			if (existingCategory == null)
			{
				throw new Exception("Güncellenecek kategori bulunamadı.");
			}

			var categoryWithSameName = await _categoryRepository.GetCategoryByName(request.Name);
			if (categoryWithSameName != null && categoryWithSameName.Id != request.Id)
			{
				throw new Exception("Bu isimde başka bir kategori zaten var.");
			}

			
			if (request.Image != null)
			{
				
				var fileStream = request.Image.OpenReadStream();
				var newImageUrl = await _imageService.UpdateImageAsync(existingCategory.Url, fileStream, request.Image.FileName, ImageType.CategoryImage);

				existingCategory.Url = newImageUrl;
			}


			existingCategory.Name = request.Name;
			existingCategory.Url = request.Url;

			_categoryRepository.Update(existingCategory);
			await _unitOfWork.SaveChangesAsync();

			return new UpdateCategoryCommandResponse();
		}
	}

	public class UpdateCategoryCommandResponse
	{
		public string Message { get; set; } = "Kategori başarıyla güncellendi";
	}
}