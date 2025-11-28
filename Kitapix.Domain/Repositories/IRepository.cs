using System.Linq.Expressions;

namespace Kitapix.Domain.Repositories
{
	public interface IRepository<T> where T : class
	{
		Task<T?> GetByIdAsync(int id);
		Task<List<T>> GetAllAsync();
		Task<IEnumerable<T>> GetAllByExpressionAsync(Expression<Func<T, bool>> expression);
		Task AddAsync(T entity);
		void Update(T entity);
		Task DeleteAsync(int id);
	}
}
