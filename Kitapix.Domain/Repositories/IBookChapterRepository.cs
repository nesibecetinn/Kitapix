using Kitapix.Domain.Entities;

namespace Kitapix.Domain.Repositories
{
	public interface IBookChapterRepository : IRepository<BookChapter>
	{
		Task<BookChapter?> GetBookChapterByMongoDbIdAsync(string id);
	}
}
