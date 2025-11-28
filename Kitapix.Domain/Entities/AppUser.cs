using Microsoft.AspNetCore.Identity;

namespace Kitapix.Domain.Entities
{
	public  class AppUser : IdentityUser<int>
	{
		public required string Name { get; set; }
		public required string SurName { get; set; }
		public string? CoverImageUrl { get; set; }

		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpires { get; set; }
		public List<Book> Books { get; set; }
		public List<Library> LibraryBooks { get; set; }
		public List<UserBookCategory> UserBookCategories { get; set; } = new();
	}
}
