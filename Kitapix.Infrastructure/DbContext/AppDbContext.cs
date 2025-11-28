using Kitapix.Domain.Abstractions;
using Kitapix.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kitapix.Infrastructure.DbContext
{
	public class AppDbContext : IdentityDbContext<AppUser, AppRole , int>
	{
		private IConfiguration _configuration;
		public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
		{
			_configuration = configuration;
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServer"));
		}

		public DbSet<Book> Books { get; set; }	
		public DbSet<Library> Libraries { get; set; }
		public DbSet<BookStats> BookStats { get; set; }
		public DbSet<BookChapter> BookChapters { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<BookCategory> BookCategories { get; set; }
		protected override void OnModelCreating(ModelBuilder builder)
		{
            base.OnModelCreating(builder);

            builder.Entity<Book>()
				.HasOne(b => b.Author)   
				.WithMany(u => u.Books)  
				.HasForeignKey(b => b.AuthorId) 
				.OnDelete(DeleteBehavior.Cascade);
			
			builder.Entity<BookChapter>()
				.HasOne(b => b.Books)
				.WithMany(u => u.Chapters)
				.HasForeignKey(b => b.BookId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Library>()
				.HasOne(ub => ub.User)
				.WithMany(u => u.LibraryBooks)
				.HasForeignKey(ub => ub.UserId)
				.OnDelete(DeleteBehavior.NoAction);
			
			builder.Entity<BookStats>()
			   .HasOne(bs => bs.Book)
			   .WithOne(b => b.Stats)
			   .HasForeignKey<BookStats>(bs => bs.BookId)
			   .OnDelete(DeleteBehavior.Cascade);

			builder.Entity<BookCategory>()
				.HasKey(bc => new { bc.BookId, bc.CategoryId });

			builder.Entity<BookCategory>()
				.HasOne(bc => bc.Book)
				.WithMany(b => b.BookCategories)
				.HasForeignKey(bc => bc.BookId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<BookCategory>()
				.HasOne(bc => bc.Category)
				.WithMany(c => c.BookCategories)
				.HasForeignKey(bc => bc.CategoryId)
				.OnDelete(DeleteBehavior.Cascade);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			foreach (var entry in ChangeTracker.Entries<Entity>())
			{
				if (entry.State == EntityState.Added)
				{
					entry.Entity.CreatedDate = DateTime.Now;
					entry.Entity.UpdatedDate = DateTime.Now;
				}
				else if (entry.State == EntityState.Modified)
				{
					entry.Entity.UpdatedDate = DateTime.Now;
				}
			}

			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
