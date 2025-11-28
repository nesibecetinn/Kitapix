using Kitapix.Domain.Repositories;
using Kitapix.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kitapix.Infrastructure.Repositories
{
	public class RepositoryBase<T> : IRepository<T> where T : class
	{
		protected readonly AppDbContext _appDbContext;
		

		public RepositoryBase(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		
		}
		protected DbSet<T> _dbSet { get => _appDbContext.Set<T>(); }  
		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public async Task DeleteAsync(int id)
		{
			var entity = await GetByIdAsync(id);
			if (entity != null)
			{
				_dbSet.Remove(entity);
			}
		}

		public async Task<List<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<IEnumerable<T>> GetAllByExpressionAsync(Expression<Func<T, bool>> expression)
		{
			return await _dbSet.Where(expression).ToListAsync();
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public void Update(T entity)
		{
			_dbSet.Update(entity);
		}
	}
}
