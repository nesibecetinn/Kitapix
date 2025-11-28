using AutoMapper;
using FluentValidation;
using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Kitapix.Application.Features.UserFeatures
{
	/// <summary>
	/// 
	///  Kullanıcıların kitapları listelemek veya filtrelemek için kullanacağı kategori kimlikleri
	///  
	/// </summary>
	public class CreateUserBookCategoriesCommand : IRequest<CreateUserBookCategoriesCommandResponse>
	{
		public List<int> CategoryIds { get; set; } = new();
	} 
	public class CreateUserBookCategoriesValidator : AbstractValidator<CreateUserBookCategoriesCommand>
	{
		public CreateUserBookCategoriesValidator()
		{
			RuleFor(x => x.CategoryIds)
		   .NotNull().WithMessage("Kategori seçimi boş olamaz.")
		   .Must(x => x.Count > 0).WithMessage("En az bir kategori seçmelisiniz.")
			.Must(x => x.Count <= 10).WithMessage("En fazla 10 kategori seçebilirsiniz.");
		}
	}
	public class CreateUserBookCategoryHandler : IRequestHandler<CreateUserBookCategoriesCommand, CreateUserBookCategoriesCommandResponse>
	{
		private readonly IUserBookCategoryRepository _userBookCategoryRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public CreateUserBookCategoryHandler(IUserBookCategoryRepository userBookCategoryRepository, 
			IUnitOfWork unitOfWork, 
			IMapper mapper,
			IHttpContextAccessor httpContextAccessor)
		{
			_userBookCategoryRepository = userBookCategoryRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}
		// Kullancının seçtiği kitap kategorilerini güncellemek içinde kullanılabilir.Once hepsini siler sonra ekler
		public async  Task<CreateUserBookCategoriesCommandResponse> Handle(CreateUserBookCategoriesCommand request, CancellationToken cancellationToken)
		{
			//var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
			//if (userIdClaim == null)
			//	throw new Exception("Kullanıcı doğrulanamadı");

			var userId = 1;
			await _userBookCategoryRepository.DeleteAllUserBookCategoryByUserId(userId);

			var newCategories = _mapper.Map<List<UserBookCategory>>(request);
			newCategories.ForEach(x => x.UserId = userId);

			await _userBookCategoryRepository.AddAllUserBookCategoryByUserId(newCategories);

			await _unitOfWork.SaveChangesAsync();

			return new CreateUserBookCategoriesCommandResponse();		
		}
	}

	public class CreateUserBookCategoriesCommandResponse
	{
		public string Message { get; set; } = "Kategoriler başarıyla seçildi";
	}
}
