using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using Kitapix.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Kitapix.Infrastructure.Repositories
{
	public class BookRepositoryBase : RepositoryBase<Book>, IBookRepository
	{
		public BookRepositoryBase(AppDbContext appDbContext) : base(appDbContext)
		{
		}
		public async Task<List<Book>> GetAllWithStatsAndAuthorAsync(List<int> categoryIds)
		{
			return await _appDbContext.Books
				.Include(x => x.Stats)
				.Include(x => x.Author)
				.Include(x => x.BookCategories)
					.ThenInclude(bc => bc.Category)
				.Where(book => book.BookCategories.Any(bc => categoryIds.Contains(bc.CategoryId)))
				.ToListAsync();
		}

		public async Task<List<Book>> GetBookByCreateDate(List<int> categoryIds)
		{
			return await _appDbContext.Books
				.Include(x => x.Stats)
				.Include(x => x.Author)
				.Include(x => x.BookCategories)
					.ThenInclude(bc => bc.Category)
				.Where(book => book.BookCategories.Any(bc => categoryIds.Contains(bc.CategoryId))).OrderByDescending(b => b.CreatedDate)
				.ToListAsync();
		}

		public async Task<List<Book>> GetBookByViewCount(List<int> categoryIds)
		{
			return await _appDbContext.Books
				.Include(x => x.Stats)
				.Include(x => x.Author)
				.Include(x => x.BookCategories)
					.ThenInclude(bc => bc.Category)
				.Where(book => book.BookCategories.Any(bc => categoryIds.Contains(bc.CategoryId))).OrderByDescending(b => b.Stats.ViewCount)
				.ToListAsync();
		}

		public async Task<List<Book>> GetBookListByUserId(int userId)
		{
			return await _appDbContext.Books
				.Include(x => x.Stats)
				.Include(x => x.Author)
				.Include(x => x.BookCategories)
					.ThenInclude(bc => bc.Category)
				.ToListAsync();
		}
	}
}
