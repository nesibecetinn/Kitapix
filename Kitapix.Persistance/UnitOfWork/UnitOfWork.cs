using Kitapix.Domain.UnitOfWork;
using Kitapix.Persistance.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Persistance.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _appDbContext;

		public UnitOfWork(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public void Dispose()
		{
			_appDbContext.Dispose();
		}

		public Task<int> SaveChangesAsync()
		{
			return _appDbContext.SaveChangesAsync();
		}
	}
}
