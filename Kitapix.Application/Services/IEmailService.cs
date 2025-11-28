using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Application.Services
{
	public interface IEmailService
	{
		Task SendEmailAsync(string to, string subject, string body);
		Task SendTemplatedEmailAsync(string to, string subject, string templateName, Dictionary<string, string> keys);
	}
}
