using ECommerce.ProductApi.Application.Interfaces;
using ECommerce.ProductApi.Infrastruture.Data;
using ECommerce.ProductApi.Infrastruture.Repositories;
using ECommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ProductApi.Infrastruture.DependencyInjection
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
		{
			SharedServiceContainer.AddSharedServices<ECommerceDbContext>(services, config, config["MySerilog:fileName"]!);
			services.AddScoped<IProduct, ProductRepository>();
			return services;
		}

		public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
		{
			SharedServiceContainer.UseSharedPolicies(app);
			return app;
		}
	}
}
