using Kitapix.Domain.Entities;

namespace Kitapix.Application.Services
{
	public interface IJwtTokenService
	{
		Task<string> GetGenerateTokenAsync(AppUser appUser);
	}
}
