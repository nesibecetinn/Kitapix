using Kitapix.Domain.UnitOfWork;
using Kitapix.Infrastructure.DbContext;

namespace Kitapix.Infrastructure.UnitOfWork
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
