using Kitapix.Domain.Abstractions;

namespace Kitapix.Domain.Entities
{
	public class Category : Entity
	{
		public string Name { get; set; }
		public string Url { get; set; }
		public List<BookCategory> BookCategories { get; set; } = new();
		public List<UserBookCategory> UserBookCategories { get; set; } = new();
	}
}
