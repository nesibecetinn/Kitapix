using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using Kitapix.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Kitapix.Infrastructure.Repositories
{
	public class BookStatRepositoryBase : RepositoryBase<BookStats>, IBookStatRepository
	{
		public BookStatRepositoryBase(AppDbContext appDbContext) : base(appDbContext)
		{
		}

		public async Task<BookStats?> GetBookStatByBookIdAsync(int bookId)
		{
			return await _dbSet.FirstOrDefaultAsync(b => b.BookId == bookId);
		   
		}
	}
}
