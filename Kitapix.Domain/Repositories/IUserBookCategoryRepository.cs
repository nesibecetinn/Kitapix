using Kitapix.Domain.Entities;

namespace Kitapix.Domain.Repositories
{
	public interface IUserBookCategoryRepository : IRepository<UserBookCategory>
	{
		Task<List<UserBookCategory>> GetUserBookCategoryByUserId(int userId);
		Task DeleteAllUserBookCategoryByUserId(int userId);
		Task AddAllUserBookCategoryByUserId(List<UserBookCategory> entities);


	}
}
