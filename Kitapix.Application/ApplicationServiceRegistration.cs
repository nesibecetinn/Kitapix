using FluentValidation;
using Kitapix.Application.Behaviors;
using Kitapix.Application.Features.AuthFeatures;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Kitapix.Application
{
	public static class ApplicationServiceRegistration
	{
		public static IServiceCollection AddApplicationService(this IServiceCollection services)
		{
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));
			
			services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();

			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
			services.AddHttpContextAccessor();
			return services;
		}
	}
}
