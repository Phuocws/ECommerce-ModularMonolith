using ECommerce.OrderApi.Application.Services;
using ECommerce.SharedLibrary.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;

namespace ECommerce.OrderApi.Application.DependecyInjection
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHttpClient<IOrderService, OrderService>(options =>
			{
				options.BaseAddress = new Uri(configuration["ApiGateway:BaseAddress"]!);
				options.Timeout = TimeSpan.FromSeconds(3);
			});

			var retryStategy = new RetryStrategyOptions
			{
				ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
				BackoffType = DelayBackoffType.Constant,
				UseJitter = true,
				MaxRetryAttempts = 3,
				Delay = TimeSpan.FromSeconds(2),
				OnRetry = arg =>
				{
					string message = $"OnRetry, Attemp: {arg.AttemptNumber} Outcome {arg.Outcome}";
					LogException.LogToConsole(message);
					LogException.LogToDebugger(message);
					return ValueTask.CompletedTask;
				}
			};

			services.AddResiliencePipeline("retry", builder =>
			{
				builder.AddRetry(retryStategy);
			});

			return services;
		}
	}
}
