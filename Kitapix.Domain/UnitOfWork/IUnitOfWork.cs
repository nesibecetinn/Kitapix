using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Domain.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{		
		Task<int> SaveChangesAsync();
	}
	
}
