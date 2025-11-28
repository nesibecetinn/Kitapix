using Kitapix.Domain.Entities.MongoEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Kitapix.Infrastructure.DbContext
{
	public class MongoDbContext 
	{
			
		private readonly IMongoDatabase _database;

		public MongoDbContext(IConfiguration configuration, IOptions<MongoDbSettings> options)
		{
			var mongoClient = new MongoClient(options.Value.ConnectionString);
			_database = mongoClient.GetDatabase(options.Value.DatabaseName);
		}
		public IMongoDatabase Database => _database;
		public IMongoCollection<BookChapter> BookChapters => _database.GetCollection<BookChapter>("BookChapters");

		public async Task SeedDataAsync()
		{				
				await BookChapters.InsertManyAsync(new List<BookChapter>
				{
					new BookChapter { Title = "Bölüm 1", Content = "Bu, ilk bölümdür." },
					new BookChapter { Title = "Bölüm 2", Content = "Bu, ikinci bölümdür." }
				});		
		}
		
	}
}
