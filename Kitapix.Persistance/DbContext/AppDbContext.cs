using Kitapix.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Persistance.DbContext
{
	public class AppDbContext : IdentityDbContext<AppUser, AppRole , int>
	{
		private IConfiguration _configuration;
		public AppDbContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServer"));
		}
		
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Ignore<IdentityRoleClaim<int>>();
			builder.Ignore<IdentityUserClaim<int>>();
			builder.Ignore<IdentityUserLogin<int>>();
			builder.Ignore<IdentityUserRole<int>>();
			builder.Ignore<IdentityUserToken<int>>();


		
		}
	}
}
