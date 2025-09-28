using ECommerce.AuthenticationApi.Application.Interfaces;
using ECommerce.AuthenticationApi.Infrastructure.Data;
using ECommerce.AuthenticationApi.Infrastructure.Repositories;
using ECommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.AuthenticationApi.Infrastructure.DependencyInjection
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
		{
			SharedServiceContainer.AddSharedServices<AuthenticationDbContext>(services, configuration, configuration["MySerilog:FileName"]!);
			services.AddScoped<IUser, UserRepository>();
			return services;
		}

		public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
		{
			SharedServiceContainer.UseSharedPolicies(app);
			return app;
		}
	}
}
