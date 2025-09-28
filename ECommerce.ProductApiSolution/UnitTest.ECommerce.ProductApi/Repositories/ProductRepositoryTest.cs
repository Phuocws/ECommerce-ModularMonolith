using ECommerce.ProductApi.Domain.Entities;
using ECommerce.ProductApi.Infrastruture.Data;
using ECommerce.ProductApi.Infrastruture.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ECommerce.ProductApi.Repositories
{
	public class ProductRepositoryTest
	{
		private readonly ECommerceDbContext context;
		private readonly ProductRepository productRepository;

		public ProductRepositoryTest()
		{
			var options = new DbContextOptionsBuilder<ECommerceDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;
			context = new ECommerceDbContext(options);
			productRepository = new ProductRepository(context);
		}

		[Fact]
		public async Task CreateAsync_ShouldAddProduct_WhenProductDoesNotExist()
		{
			// Arrange
			var product = new Product
			{
				Name = "Test Product",
				Price = 100.00m,
				Quantity = 10
			};
			// Act
			var result = await productRepository.CreateAsync(product);
			// Assert
			Assert.True(result.flag);
			Assert.Equal("Test Product added to database successfully.", result.message);
			Assert.NotNull(await context.Products.FindAsync(product.Id));
		}

		[Fact] 
		public async Task GetByConditionAsync_WhenProductExists_ShouldReturnProduct()
		{
			// Arrange
			var product = new Product
			{
				Name = "Test Product",
				Price = 100.00m,
				Quantity = 10
			};
			await context.Products.AddAsync(product);
			await context.SaveChangesAsync();
			// Act
			var result = await productRepository.GetByConditionAsync(p => p.Name == "Test Product");
			// Assert
			Assert.NotNull(result);
			Assert.Equal("Test Product", result.Name);
		}
	}
}
