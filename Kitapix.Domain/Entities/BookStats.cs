using Kitapix.Domain.Abstractions;

namespace Kitapix.Domain.Entities
{
	public class BookStats : Entity
	{
		public int ViewCount { get; set; }
		public int LikeCount { get; set; }

		public int BookId { get; set; }

		public Book Book { get; set; }

	}
}
