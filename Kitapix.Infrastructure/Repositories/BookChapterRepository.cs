using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using Kitapix.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Kitapix.Infrastructure.Repositories
{
	public class BookChapterRepository : RepositoryBase<BookChapter>, IBookChapterRepository
	{
		public BookChapterRepository(AppDbContext appDbContext) : base(appDbContext)
		{
		}

		public async Task<BookChapter?> GetBookChapterByMongoDbIdAsync(string id)
		{
			return await _dbSet.FirstOrDefaultAsync(x=>x.MongoDbId == id);
		}
	}
}
