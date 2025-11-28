using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using Kitapix.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Kitapix.Infrastructure.Repositories
{
	public class CategoryRepositoryBase : RepositoryBase<Category>, ICategoryRepository
	{
		public CategoryRepositoryBase(AppDbContext appDbContext) : base(appDbContext)
		{
		}

		public async Task<Category> GetCategoryByName(string Name)
		{
			return await _dbSet.FirstOrDefaultAsync(x => x.Name == Name);
		}
	}
}
