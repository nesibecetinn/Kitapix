using FluentValidation;
using System.Net;

namespace Kitapix.WebAPI.Middlewares.Exceptionhandler
{
	public class ExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(httpContext, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";

			if (exception is ValidationException validationException)
			{
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

				var response = new
				{
					Message = "Geçersiz veri girişi",
					Errors = validationException.Errors
				   .GroupBy(e => e.PropertyName) // Aynı alan için birden fazla hata varsa grupla
				   .ToDictionary(
					   g => g.Key,
					   g => g.Select(e => e.ErrorMessage).Distinct().ToList() // Hata mesajlarını al
				   )
				};
				return context.Response.WriteAsJsonAsync(response);
			}
			if (exception is UnauthorizedAccessException)
			{
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				var response = new { Message = "Yetkisiz erişim" };
				return context.Response.WriteAsJsonAsync(response);
			}
			if (exception is KeyNotFoundException)
			{
				context.Response.StatusCode = (int)HttpStatusCode.NotFound;
				var response = new { Message = exception.Message };
				return context.Response.WriteAsJsonAsync(response);
			}
			// Genel hata mesajı
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var errorResponse = new
			{
				Message = exception.Message,
				StackTrace = exception.StackTrace
			};

			return context.Response.WriteAsJsonAsync(errorResponse);
		}
	}
}
