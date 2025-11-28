using Kitapix.Domain.Abstractions;
using Kitapix.Domain.Repositories.MongoDbRepositoreis;
using Kitapix.Infrastructure.DbContext;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;


namespace Kitapix.Infrastructure.Repositories.MongoRepositories
{
	public class MongoRepositoryBase<T> : IMongoRepository<T> where T : class
	{
		protected readonly IMongoCollection<T> _collection;

		public MongoRepositoryBase(MongoDbContext context)
		{
			string collectionName = context.BookChapters.CollectionNamespace.CollectionName;
			_collection = context.Database.GetCollection<T>(collectionName);
		}

		public async Task AddAsync(T entity)
		{
			await _collection.InsertOneAsync(entity);
		}

		public async Task DeleteAsync(string id)
		{		
			var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
		
			await _collection.DeleteOneAsync(filter);
		}

		public async Task<List<T>> GetAllAsync()
		{
			return await _collection.Find(_ => true).ToListAsync();
		}

		public async Task<IEnumerable<T>> GetAllByExpressionAsync(Expression<Func<T, bool>> expression)
		{
			return await _collection.Find(expression).ToListAsync();
		}

		public async Task<T?> GetByIdAsync(string id)
		{		
			var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
			return await _collection.Find(filter).FirstOrDefaultAsync();
		}

		public void Update(T entity)
		{

			var id = entity.GetType().GetProperty("Id")?.GetValue(entity).ToString();
			var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));		
			var result = _collection.ReplaceOne(filter, entity);
		}
	}
}
