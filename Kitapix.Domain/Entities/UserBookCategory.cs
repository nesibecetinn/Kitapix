using Kitapix.Domain.Abstractions;

namespace Kitapix.Domain.Entities
{
	public class UserBookCategory :Entity
	{
		public int UserId { get; set; }
		public AppUser User { get; set; }

		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
