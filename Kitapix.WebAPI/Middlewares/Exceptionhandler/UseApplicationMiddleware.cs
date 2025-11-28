namespace Kitapix.WebAPI.Middlewares.Exceptionhandler
{
	public static class UseApplicationMiddleware
	{
		public static IApplicationBuilder UseApplicationMiddlewares(this IApplicationBuilder builder)
		{

			builder.UseMiddleware<ExceptionHandlerMiddleware>();
			return builder;
		}
	}
}
