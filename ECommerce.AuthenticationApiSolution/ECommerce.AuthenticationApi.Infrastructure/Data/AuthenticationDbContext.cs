using ECommerce.AuthenticationApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AuthenticationApi.Infrastructure.Data
{
	public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : DbContext(options)
	{
		public DbSet<AppUser> Users { get; set; }
	}
}
