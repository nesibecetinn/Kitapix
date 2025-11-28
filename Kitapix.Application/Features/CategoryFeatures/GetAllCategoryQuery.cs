using AutoMapper;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using MediatR;

namespace Kitapix.Application.Features.CategoryFeatures
{
	public class GetAllCategoryQuery : IRequestWithoutValidator<List<GetAllCategoryQueryResponse>>
	{
	}
	public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, List<GetAllCategoryQueryResponse>>
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;
	
		public GetAllCategoryQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
	
		}
		public async Task<List<GetAllCategoryQueryResponse>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
		{
			var existingcategory = await _categoryRepository.GetAllAsync();
			if (existingcategory == null)
			{
				throw new Exception("Kategori bulunamadı");
			}
			List<GetAllCategoryQueryResponse> response = _mapper.Map<List<GetAllCategoryQueryResponse>>(existingcategory);
			return response;		
		}
	}

	public class GetAllCategoryQueryResponse
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
	}
}
