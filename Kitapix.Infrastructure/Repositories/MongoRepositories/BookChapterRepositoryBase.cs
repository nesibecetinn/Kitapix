using Kitapix.Domain.Entities.MongoEntities;
using Kitapix.Domain.Repositories.MongoDbRepositoreis;
using Kitapix.Infrastructure.DbContext;
using MongoDB.Driver;

namespace Kitapix.Infrastructure.Repositories.MongoRepositories
{
	public class BookChapterRepositoryBase : MongoRepositoryBase<BookChapter>, IBookChapterRepository
	{
		public BookChapterRepositoryBase(MongoDbContext context) : base(context)
		{
		}


		public async Task<List<BookChapter>> GetChaptersByBookIdAsync(int id)
		{
			var filter = Builders<BookChapter>.Filter.Eq("BookId", id);
			return await _collection.Find(filter).ToListAsync();
		}
	
	}
}
