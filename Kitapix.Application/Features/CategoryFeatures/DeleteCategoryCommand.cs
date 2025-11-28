using AutoMapper;
using FluentValidation;
using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using Kitapix.Domain.Enums;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using MediatR;

namespace Kitapix.Application.Features.CategoryFeatures
{
	public class DeleteCategoryCommand : IRequest<DeleteCategoryCommandResponse>
	{
		public int Id { get; set; }
	}
	public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
	{
		public DeleteCategoryCommandValidator() {
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id alanı boş olamaz");
		}
	}
	public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, DeleteCategoryCommandResponse>
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IImageService _imageService; // 📷 Resim servisini ekledik

		public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IImageService imageService)
		{
			_categoryRepository = categoryRepository;
			_unitOfWork = unitOfWork;
			_imageService = imageService;
		}

		public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
		{
			var existingCategory = await _categoryRepository.GetByIdAsync(request.Id);
			if (existingCategory == null)
			{
				throw new Exception("Kategori bulunamadı");
			}

	
			if (!string.IsNullOrEmpty(existingCategory.Url))
			{
				await _imageService.DeleteImageAsync(existingCategory.Url);
			}

			await _categoryRepository.DeleteAsync(request.Id);
			await _unitOfWork.SaveChangesAsync();

			return new DeleteCategoryCommandResponse();
		}
	}

	public class DeleteCategoryCommandResponse
	{
		public string Message { get; set; } = "Kategori başarıyla silindi";
	}
}
