using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Application.Services
{
	public interface IUserBookCategoryService
	{
		
		Task<List<int>> GetUserCategoryIdsAsync();
	}
}

