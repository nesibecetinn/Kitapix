using Kitapix.Domain.Abstractions;
using Kitapix.Domain.Entities.MongoEntities;

namespace Kitapix.Domain.Entities
{
	public class Book : Entity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public int AuthorId { get; set; }
		public string? CoverImageUrl { get; set; }	
		public AppUser Author { get; set; }
		public List<BookChapter> Chapters { get; set; }
		public List<Library> LibraryUsers { get; set; }
		public BookStats Stats { get; set; } = new BookStats();
		public List<BookCategory> BookCategories { get; set; } = new();
	}
}
