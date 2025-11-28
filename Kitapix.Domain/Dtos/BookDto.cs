using Kitapix.Domain.Entities;
using BookChapter = Kitapix.Domain.Entities.MongoEntities.BookChapter;

namespace Kitapix.Domain.Dtos
{
	public class BookDto
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int AuthorId { get; set; }
		public string AuthorName { get; set; }
		public string AuthorSurName { get; set; }
		public int ViewCount { get; set; }
		public int LikeCount { get; set; }
		public List<BookChapter> Chapters { get; set; }

		public List<string> CategoryName { get; set; }
	}
}
