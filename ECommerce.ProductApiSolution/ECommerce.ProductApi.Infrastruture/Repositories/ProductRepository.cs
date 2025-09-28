using ECommerce.ProductApi.Application.Interfaces;
using ECommerce.ProductApi.Domain.Entities;
using ECommerce.ProductApi.Infrastruture.Data;
using ECommerce.SharedLibrary.Logs;
using ECommerce.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ProductApi.Infrastruture.Repositories
{
	public class ProductRepository(ECommerceDbContext context) : IProduct
	{
		public async Task<Response> CreateAsync(Product entity)
		{
			try
			{
				var getProduct = await GetByConditionAsync(_ => _.Name!.Equals(entity.Name));
				if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
				{
					return new Response
					(
						false,
						$"{entity.Name} Product already exists."
					);
				}
				var currentEntity = context.Products.Add(entity).Entity;
				await context.SaveChangesAsync();

				if (currentEntity is not null && currentEntity.Id > 0)
				{
					return new Response(true, $"{entity.Name} added to database successfully.");
				}
				else
				{
					return new Response(false, $"Error occurred while adding {entity.Name}.");
				}
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				return new Response(false, "Error occurred adding new product.");
			}
		}

		public async Task<Response> DeleteAsync(int id)
		{
			try
			{
				var product = await GetByIdAsync(id);
				if (product is null)
				{
					return new Response(false, $"{product?.Name} not found.");
				}
				context.Products.Remove(product);
				await context.SaveChangesAsync();
				return new Response(true, $"{product?.Name} deleted successfully.");
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				return new Response(false, "Error occurred deleting product.");
			}
		}

		public async Task<IEnumerable<Product>> GetAllAsync()
		{
			try
			{
				var products = await context.Products.AsNoTracking().ToListAsync();
				return products is not null ? products : null!;
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("Error occurred retrieving products.");
			}
		}

		public async Task<Product> GetByConditionAsync(Expression<Func<Product, bool>> predicate)
		{
			try
			{
				var product = await context.Products.Where(predicate).FirstOrDefaultAsync();
				return product is not null ? product : null!;
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("Error occurred retrieving product.");
			}
		}

		public async Task<Product> GetByIdAsync(int id)
		{
			try
			{
				var product = await context.Products.FindAsync(id);
				return product is not null ? product : null!;
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("Error occurred retrieving product.");
			}
		}

		public async Task<Response> UpdateAsync(Product entity)
		{
			try
			{
				var product = await GetByIdAsync(entity.Id);
				if (product is null)
				{
					return new Response(false, $"{entity.Name} not found.");
				}
				context.Entry(product).State = EntityState.Detached;
				context.Products.Update(product);
				await context.SaveChangesAsync();
				return new Response(true, $"{entity.Name} updated successfully.");
			}
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				return new Response(false, "Error occurred updating product.");
			}
		}
	}
}
