using Microsoft.AspNetCore.Http;

namespace ECommerce.SharedLibrary.Middleware
{
	public class ListenToOnlyApiGateway(RequestDelegate next)
	{
		public async Task InvokeAsync(HttpContext context)
		{
			var signedHeader = context.Request.Headers["Api-Gateway"];
			if (signedHeader.FirstOrDefault() is null)
			{
				context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
				await context.Response.WriteAsync("503 Service Unavailable");
				return;
			}
			else
			{
				await next(context);
			}
		}
	}
}
