using Kitapix.Application.Services;
using Kitapix.Domain.Entities;
using Kitapix.Domain.Repositories;
using Kitapix.Domain.UnitOfWork;
using Kitapix.Infrastructure.Authentication.Jwt;
using Kitapix.Infrastructure.DbContext;
using Kitapix.Infrastructure.Mapping;
using Kitapix.Infrastructure.Repositories;
using Kitapix.Infrastructure.Repositories.MongoRepositories;
using Kitapix.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Kitapix.Infrastructure
{
	public static class InfrustructureServiceRegistration
	{
		public static IServiceCollection AddInfrastructorService(this IServiceCollection services, IConfiguration configuration)
		{
			
			services.AddIdentity<AppUser, AppRole>(options =>
			{
				options.SignIn.RequireConfirmedEmail = true; // E-posta doğrulama gereksinimi
			}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
			  
			services.AddDbContext<AppDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

			services.AddSingleton<MongoDbContext>();

			services.AddAutoMapper(typeof(MappingProfile));
			
			services.AddScoped<IUnitOfWork, Kitapix.Infrastructure.UnitOfWork.UnitOfWork>();

			services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
			services.AddScoped<IUserRepository, UserRepositoryBase>();
			services.AddScoped<IBookRepository, BookRepositoryBase>();
			services.AddScoped<IUserBookCategoryRepository, UserBookCategoryRepositoryBase>();
			services.AddScoped<IBookStatRepository, BookStatRepositoryBase>(); 
			services.AddScoped<Domain.Repositories.IBookChapterRepository, Kitapix.Infrastructure.Repositories.BookChapterRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepositoryBase>();
			services.AddScoped<IImageService, ImageService>();
			services.AddScoped<IUserBookCategoryService , UserBookCategoryService>();

			services.Configure<MongoDbSettings>(configuration.GetSection("MongoDB"));
		
			services.AddScoped<Domain.Repositories.MongoDbRepositoreis.IBookChapterRepository, Kitapix.Infrastructure.Repositories.MongoRepositories.BookChapterRepositoryBase>();

			services.AddScoped<IEmailService, EmailService>();
			
			services.AddScoped<IJwtTokenService, JwtTokenService>();
			
			services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = jwtSettings!.Issuer,
						ValidAudience = jwtSettings.Audience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
					};
				}).AddGoogle(options =>
				{
					options.ClientId = configuration["Google:ClientId"]!;
					options.ClientSecret = configuration["Google:ClientSecret"]!;
				});

			return services;
		}
	}
}
