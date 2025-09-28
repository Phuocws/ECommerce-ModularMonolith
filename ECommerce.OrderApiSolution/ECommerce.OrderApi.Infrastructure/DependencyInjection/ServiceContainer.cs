using ECommerce.OrderApi.Application.Interfaces;
using ECommerce.OrderApi.Infrastructure.Data;
using ECommerce.OrderApi.Infrastructure.Repositories;
using ECommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.OrderApi.Infrastructure.DependencyInjection
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
		{
			SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog:fileName"]!);
			services.AddScoped<IOrder, OrderRepository>();
			return services;
		}

		public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
		{
			SharedServiceContainer.UseSharedPolicies(app);
			return app;
		}
	}
}
