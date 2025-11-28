using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using Kitapix.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Kitapix.Infrastructure.Repositories
{
	public class UserBookCategoryRepositoryBase : RepositoryBase<UserBookCategory>, IUserBookCategoryRepository
	{
		public UserBookCategoryRepositoryBase(AppDbContext appDbContext) : base(appDbContext)
		{
		}

		public async Task AddAllUserBookCategoryByUserId(List<UserBookCategory> entities)
		{
			await _dbSet.AddRangeAsync(entities);
		}

		public async Task DeleteAllUserBookCategoryByUserId(int userId)
		{
			var userBookCategory = await this.GetUserBookCategoryByUserId(userId);
			_dbSet.RemoveRange(userBookCategory);
		}

		public async Task<List<UserBookCategory>> GetUserBookCategoryByUserId(int userId)
		{
			return await _dbSet.Where(x => x.UserId == userId).ToListAsync();
		}
	}
}
