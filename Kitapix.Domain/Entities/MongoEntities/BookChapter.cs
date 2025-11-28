using Kitapix.Domain.Abstractions;
using MongoDB.EntityFrameworkCore;

namespace Kitapix.Domain.Entities.MongoEntities
{
	[Collection("book-chapters")]
	public class BookChapter : MongoEntity
	{
	
		public int BookId { get; set; }
		public required string Title { get; set; }
		public required string Content { get; set; }
	}
}
