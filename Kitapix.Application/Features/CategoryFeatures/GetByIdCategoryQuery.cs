using AutoMapper;
using Kitapix.Application.Features.BookFeatures;
using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Application.Features.CategoryFeatures
{
	public class GetByIdCategoryQuery :IRequestWithoutValidator<GetByIdCategoryQueryResponse>
	{
		public int Id { get; set; }
	}
	public class GetByIdCategoryQueryHandler : IRequestHandler<GetByIdCategoryQuery, GetByIdCategoryQueryResponse>
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;
		public GetByIdCategoryQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}
		public async Task<GetByIdCategoryQueryResponse> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
		{
			var existingcategory = await _categoryRepository.GetByIdAsync(request.Id);
			if (existingcategory == null)
			{
				throw new Exception("Kategori bulunamadı");
			}
			GetByIdCategoryQueryResponse response = _mapper.Map<GetByIdCategoryQueryResponse>(existingcategory);
			return response;
		}
	}

	public class GetByIdCategoryQueryResponse
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
	}
}
