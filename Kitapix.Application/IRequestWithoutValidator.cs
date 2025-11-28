using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Application
{
	public interface IRequestWithoutValidator<TResponse> : IRequest<TResponse> { }
}
