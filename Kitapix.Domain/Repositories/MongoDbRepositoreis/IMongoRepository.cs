using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Domain.Repositories.MongoDbRepositoreis
{
	public interface IMongoRepository<T> where T : class
	{
		Task<T?> GetByIdAsync(string id);
		Task<List<T>> GetAllAsync();
		Task<IEnumerable<T>> GetAllByExpressionAsync(Expression<Func<T, bool>> expression);
		Task AddAsync(T entity);
		void Update(T entity);
		Task DeleteAsync(string id);
	}
}
