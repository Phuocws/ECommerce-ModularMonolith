using ECommerce.ProductApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ProductApi.Infrastruture.Data
{
	// fix to ProductDbContext instead of ECommerceDbContext
	public class ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : DbContext(options)
	{
		public DbSet<Product> Products { get; set; }
	}
}
