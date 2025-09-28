using ECommerce.AuthenticationApi.Application.DTOs;
using ECommerce.AuthenticationApi.Application.Interfaces;
using ECommerce.AuthenticationApi.Domain.Entities;
using ECommerce.AuthenticationApi.Infrastructure.Data;
using ECommerce.SharedLibrary.Logs;
using ECommerce.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.AuthenticationApi.Infrastructure.Repositories
{
	public class UserRepository(AuthenticationDbContext context, IConfiguration config) : IUser
	{
		private async Task<AppUser> GetUserByEmail(string email)
		{
			try
			{
				var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
				return user is not null ? user : null!;
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("An error occurred while fetching the user by email");
			}
		}

		public async Task<GetUserDTO> GetUserById(int id)
		{
			try
			{
				var user = await context.Users.FindAsync(id);
				return (user is not null) ? new GetUserDTO
					(
						user.Id,
						user.UserName!,
						user.Email!,
						user.PhoneNumber!,
						user.Address!,
						user.Role!
					) : null!;
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("An error occurred while fetching the user");
			}
		}

		private string GenerateToken(AppUser user)
		{
			var key = Encoding.UTF8.GetBytes(config["Authentication:Key"]!);
			var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key);
			var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
			var claims = new List<Claim>
			{
				new (ClaimTypes.Name, user.UserName!),
				new (ClaimTypes.Email, user.Email!)
			};
			if (user.Role is not null)
				claims.Add(new Claim(ClaimTypes.Role, user.Role));

			var token = new JwtSecurityToken(
				issuer: config["Authentication:Issuer"],
				audience: config["Authentication:Audience"],
				expires: DateTime.UtcNow.AddHours(1),
				claims: claims,
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<Response> Login(LoginDTO loginDTO)
		{
			try
			{
				var user = await GetUserByEmail(loginDTO.Email);
				if (user is null)
					return new Response(false, "Invalid credentials");

				bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Password!);
				if (!isValidPassword)
					return new Response(false, "Invalid credentials");

				var token = GenerateToken(user);
				return new Response(true, token);
			}
			catch (Exception ex)
			{
				return new Response(false, $"An error occurred: {ex.Message}");
			}
		}

		public async Task<Response> Register(AppUserDTO appUserDTO)
		{
			try
			{
				if (await GetUserByEmail(appUserDTO.Email) is not null)
					return new Response(false, "User already exists");

				var user = new AppUser()
				{
					UserName = appUserDTO.Name,
					Email = appUserDTO.Email,
					Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password),
					PhoneNumber = appUserDTO.PhoneNumber,
					Role = appUserDTO.Role,
					CreatedAt = DateTime.UtcNow,
					Address = appUserDTO.Address
				};

				var result = await context.Users.AddAsync(user);
				await context.SaveChangesAsync();
				return (result.Entity.Id > 0) ? new Response(true, "User registered successfully")
											: new Response(false, "Error occurred while register.");
			}
			catch (Exception ex)
			{
				return new Response(false, $"An error occurred: {ex.Message}");
			}
		}
	}
}
