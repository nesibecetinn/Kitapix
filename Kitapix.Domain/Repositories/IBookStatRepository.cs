using Kitapix.Domain.Entities;

namespace Kitapix.Domain.Repositories
{
	public interface IBookStatRepository:IRepository<BookStats>
	{
		Task<BookStats?> GetBookStatByBookIdAsync(int bookId);
	}
}
