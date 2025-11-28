using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitapix.Application.Services
{
	public interface ICloudflareService
	{
		Task UploadFileAsync(string bookId, string chapterName, string content);
		Task<string> GetFileUrlAsync(string bookId, string chapterName);
	}
}
