using ECommerce.OrderApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.Infrastructure.Data
{
	public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
	{
		public DbSet<Order> Orders { get; set; }
	}
}
