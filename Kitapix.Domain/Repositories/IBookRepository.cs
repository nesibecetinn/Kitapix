using Kitapix.Domain.Entities;

namespace Kitapix.Domain.Repositories
{
	public interface IBookRepository : IRepository<Book>
	{
		Task<List<Book>> GetAllWithStatsAndAuthorAsync(List<int> categoryIds);
		Task<List<Book>> GetBookByViewCount(List<int> categoryIds);
		Task<List<Book>> GetBookByCreateDate(List<int> categoryIds);
		Task<List<Book>> GetBookListByUserId(int userId);
	}
}
