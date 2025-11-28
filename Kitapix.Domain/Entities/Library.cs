using Kitapix.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Domain.Entities
{
	public class Library :Entity
	{
		public int UserId { get; set; }
		public AppUser User { get; set; }

		public int BookId { get; set; }
		public Book Book { get; set; }
	}
}
