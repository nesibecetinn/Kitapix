using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using Kitapix.Infrastructure.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Infrastructure.Repositories
{
	public class UserRepositoryBase : RepositoryBase<AppUser>, IUserRepository
	{
		public UserRepositoryBase(AppDbContext appDbContext) : base(appDbContext)
		{
		}
	}
}
