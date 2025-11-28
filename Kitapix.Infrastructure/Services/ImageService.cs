using Amazon.S3;
using Amazon.S3.Model;
using Kitapix.Application.Services;
using Kitapix.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Kitapix.Infrastructure.Services
{
	public class ImageService : IImageService
	{
		private readonly IWebHostEnvironment _env;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ImageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
		{
			_env = env;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<string> UploadImageAsync(Stream fileStream, string fileName, ImageType type)
		{
			// Dosya uzantısı ve benzersiz isim
			var ext = Path.GetExtension(fileName);
			var newFileName = $"{Guid.NewGuid()}{ext}";
			var folderName = type switch
			{
				ImageType.BookCover => "bookcovers",
				ImageType.CategoryImage => "categories",
				ImageType.UserProfile => "profiles",
				_ => "others"
			};

			var uploadPath = Path.Combine(_env.WebRootPath, "uploads", folderName);
			if (!Directory.Exists(uploadPath))
				Directory.CreateDirectory(uploadPath);

			var fullPath = Path.Combine(uploadPath, newFileName);

			// Dosyayı kaydet
			using (var fileStreamToSave = new FileStream(fullPath, FileMode.Create))
			{
				await fileStream.CopyToAsync(fileStreamToSave);
			}

			// URL oluştur
			var baseUrl = _httpContextAccessor.HttpContext?.Request.Scheme + "://" +
						  _httpContextAccessor.HttpContext?.Request.Host;
			var fileUrl = $"{baseUrl}/uploads/{folderName}/{newFileName}";

			return fileUrl;
		}

		public Task DeleteImageAsync(string imageUrl)
		{
			try
			{
				var baseUrl = _httpContextAccessor.HttpContext?.Request.Scheme + "://" +
							  _httpContextAccessor.HttpContext?.Request.Host;
				var relativePath = imageUrl.Replace($"{baseUrl}/", "");
				var fullPath = Path.Combine(_env.WebRootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

				if (File.Exists(fullPath))
				{
					File.Delete(fullPath);
				}
			}
			catch
			{
				// logla istersen
			}

			return Task.CompletedTask;
		}

		public async Task<string> UpdateImageAsync(string existingUrl, Stream fileStream, string fileName, ImageType imageType)
		{
			// Önce eski resmi sil
			if (!string.IsNullOrEmpty(existingUrl))
			{
				await DeleteImageAsync(existingUrl);
			}

			// Sonra yeni resmi yükle
			var newImageUrl = await UploadImageAsync(fileStream, fileName, imageType);

			return newImageUrl;
		}
	}
}
