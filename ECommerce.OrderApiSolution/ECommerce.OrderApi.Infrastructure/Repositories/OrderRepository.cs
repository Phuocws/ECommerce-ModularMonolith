using ECommerce.OrderApi.Application.Interfaces;
using ECommerce.OrderApi.Domain.Entities;
using ECommerce.OrderApi.Infrastructure.Data;
using ECommerce.SharedLibrary.Logs;
using ECommerce.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.Infrastructure.Repositories
{
	public class OrderRepository(OrderDbContext context) : IOrder
	{
		public async Task<Response> CreateAsync(Order entity)
		{
			try
			{
				var order = await GetByIdAsync(entity.Id);
				if (order is not null)
				{
					return new Response(false, "Order already exists.");
				}
				else
				{
					await context.Orders.AddAsync(entity);
					await context.SaveChangesAsync();
					return new Response(true, "Order created successfully.");
				}
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				return new Response(false, "Error occurred while create new order.");
			}
		}

		public async Task<Response> DeleteAsync(int id)
		{
			try
			{
				var order = await GetByIdAsync(id);
				if (order is null)
				{
					return new Response(false, "Order not found.");
				}
				else
				{
					context.Orders.Remove(order);
					await context.SaveChangesAsync();
					return new Response(true, "Order deleted successfully.");
				}
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				return new Response(false, "Error occurred while delete order.");
			}
		}

		public async Task<IEnumerable<Order>> GetAllAsync()
		{
			try
			{
				var orders = await context.Orders.AsNoTracking().ToListAsync();
				return (orders is null) ? null! : orders;
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("Error occurred while get all orders.");
			}
		}

		public async Task<Order> GetByConditionAsync(Expression<Func<Order, bool>> predicate)
		{
			try
			{
				var order = await context.Orders.Where(predicate).FirstOrDefaultAsync();
				return (order is null) ? null! : order;
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("Error occurred while get order by condition.");
			}
		}

		public async Task<Order> GetByIdAsync(int id)
		{
			try
			{
				var order = await context.Orders.FindAsync(id);
				return (order is null) ? null! : order;
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("Error occurred while get order by id.");
			}
		}

		public async Task<IEnumerable<Order>> GetOrdersByAsync(Expression<Func<Order, bool>> predicate)
		{
			try
			{
				var orders = await context.Orders.Where(predicate).AsNoTracking().ToListAsync();
				return (orders is null) ? null! : orders;
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("Error occurred while get orders by condition.");
			}
		}

		public async Task<Response> UpdateAsync(Order entity)
		{
			try
			{
				var order = await GetByIdAsync(entity.Id);
				if (order is null)
				{
					return new Response(false, "Order not found.");
				}
				else
				{
					context.Entry(order).State = EntityState.Detached;
					context.Orders.Update(entity);
					await context.SaveChangesAsync();
					return new Response(true, "Order updated successfully.");
				}
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				return new Response(false, "Error occurred while update order.");
			}
		}
	}
}
