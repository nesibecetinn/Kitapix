using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using Kitapix.Persistance.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Persistance.Repositories
{
	public class UserRepositoryBase : RepositoryBase<AppUser>, IUserRepository
	{
		public UserRepositoryBase(AppDbContext appDbContext) : base(appDbContext)
		{
		}
	}
}
