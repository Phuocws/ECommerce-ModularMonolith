using ECommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ECommerce.SharedLibrary.Middleware
{
	public class GlobalException(RequestDelegate next)
	{
		public async Task InvokeAsync(HttpContext context)
		{
			string message = "sorry, internal server error occurred. Kindly try again";
			int statusCode = StatusCodes.Status500InternalServerError;
			string title = "Internal Server Error";
			try
			{
				await next(context);

				if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
				{
					title = "Too Many Requests";
					message = "You have made too many requests in a short period. Please try again later.";
					statusCode = StatusCodes.Status429TooManyRequests;
					await MotifyHeader(context, title, message, statusCode);
				}

				if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
				{
					title = "Unauthorized";
					message = "You are not authorized to access this resource.";
					statusCode = StatusCodes.Status401Unauthorized;
					await MotifyHeader(context, title, message, statusCode);
				}

				if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
				{
					title = "Forbidden";
					message = "You do not have permission to access this resource.";
					statusCode = StatusCodes.Status403Forbidden;
					await MotifyHeader(context, title, message, statusCode);
				}
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);

				if (ex is TaskCanceledException || ex is TimeoutException)
				{
					title = "Request Timeout";
					message = "The request has timed out. Please try again later.";
					statusCode = StatusCodes.Status408RequestTimeout;
				}
				await MotifyHeader(context, title, message, statusCode);
			}
		}

		private async Task MotifyHeader(HttpContext context, string title, string message, int statusCode)
		{
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails
			{
				Detail = message,
				Status = statusCode,
				Title = title,
			}), CancellationToken.None);
		}
	}
}
