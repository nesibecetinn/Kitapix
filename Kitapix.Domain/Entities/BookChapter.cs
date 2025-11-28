using Kitapix.Domain.Abstractions;

namespace Kitapix.Domain.Entities
{
	public class BookChapter : Entity
	{
		public string MongoDbId { get; set; }
		public int BookId { get; set; }
		public Book Books { get; set; }
	}
}
