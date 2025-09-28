using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerce.SharedLibrary.DependencyInjection
{
	public static class JWTAuthenticationScheme
	{
		public static IServiceCollection AddJWTAuthenticationScheme(this IServiceCollection services, IConfiguration config)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer("Bearer", options =>
				{
					var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
					string issuer = config.GetSection("Authentication:Issuer").Value!;
					string audience = config.GetSection("Authentication:Audience").Value!;

					options.RequireHttpsMetadata = false;
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(key),
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidIssuer = issuer,
						ValidAudience = audience,
						ValidateLifetime = false
					};
				});
			return services;
		}
	}
}
