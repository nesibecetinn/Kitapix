using Kitapix.Application.Services;
using Kitapix.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Kitapix.Infrastructure.Services
{
	public class UserBookCategoryService : IUserBookCategoryService
	{
		private readonly IUserBookCategoryRepository _userBookCategoryRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserBookCategoryService(IUserBookCategoryRepository userBookCategoryRepository, IHttpContextAccessor httpContextAccessor)
		{
			_userBookCategoryRepository = userBookCategoryRepository;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<List<int>> GetUserCategoryIdsAsync()
		{
			//var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
			var userId = 1;  //Int32.Parse(userIdClaim);
			var categoryIds = (await _userBookCategoryRepository.GetUserBookCategoryByUserId(userId))
				.Select(x => x.CategoryId)
				.ToList();

			return categoryIds;
		}
	}
}
