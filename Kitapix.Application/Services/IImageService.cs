using Kitapix.Domain.Enums;

namespace Kitapix.Application.Services
{
	public interface IImageService
	{
		Task<string> UploadImageAsync(Stream fileStream, string fileName, ImageType imageType);
		Task<string> UpdateImageAsync(string existingUrl, Stream fileStream, string fileName, ImageType imageType);
		Task DeleteImageAsync(string imageUrl);
	}
}
