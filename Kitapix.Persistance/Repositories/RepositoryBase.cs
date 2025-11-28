using Kitapix.Domain.Abstractions;
using Kitapix.Domain.Repositories;
using Kitapix.Persistance.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Persistance.Repositories
{
	public class RepositoryBase<T> : IRepository<T> where T : class
	{
		private readonly AppDbContext _appDbContext;
		

		public RepositoryBase(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		
		}
		private  DbSet<T> _dbSet { get => _appDbContext.Set<T>(); }  
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

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public Task<IEnumerable<T>> GetAllByExpressionAsync(Expression<Func<T, bool>> expression)
		{
			throw new NotImplementedException();
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
