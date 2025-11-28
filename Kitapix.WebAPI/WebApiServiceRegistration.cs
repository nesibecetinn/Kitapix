using Kitapix.WebAPI.Middlewares.Exceptionhandler;
using Microsoft.OpenApi.Models;

namespace Kitapix.WebAPI
{
	public static class WebApiServiceRegistration
	{
		public static IServiceCollection AddWebApiService(this IServiceCollection services)
		{
			services.AddControllers();
			// CORS Policy
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					builder =>
					{
						builder.AllowAnyOrigin()
							   .AllowAnyMethod()
							   .AllowAnyHeader();
					});
			});

			services.AddEndpointsApiExplorer(); // Gerekli!
			
			services.AddOpenApi();
			return services;
		}
	}
}
