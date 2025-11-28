using Kitapix.Domain.Entities.MongoEntities;

namespace Kitapix.Domain.Repositories.MongoDbRepositoreis
{
	public interface IBookChapterRepository : IMongoRepository<BookChapter>
	{
		Task<List<BookChapter>> GetChaptersByBookIdAsync(int id);
	}
}
