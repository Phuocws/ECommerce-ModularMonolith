using AutoMapper;
using ECommerce.ProductApi.Application.DTOs;
using ECommerce.ProductApi.Application.Interfaces;
using ECommerce.ProductApi.Domain.Entities;
using ECommerce.ProductApi.Presentation.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace UnitTest.ECommerce.ProductApi.Controllers
{
	public class ProductControllerTest
	{
		private readonly IProduct productInterface;
		private readonly IMapper mapperInterface;
		private readonly ProductsController productController;

		public ProductControllerTest()
		{
			productInterface = A.Fake<IProduct>();
			mapperInterface = A.Fake<IMapper>();
			productController = new ProductsController(productInterface, mapperInterface);
		}

		[Fact]
		public async Task GetProducts_WhenProductExists_ReturnsOkResultWithProducts()
		{
			// Arrange
			var products = new List<Product>
			{
				new Product { Id = 1, Name = "Test Product 1", Price = 100.00m, Quantity = 10 },
				new Product { Id = 2, Name = "Test Product 2", Price = 200.00m, Quantity = 20 }
			};
			var productDTOs = new List<ProductDTO>
			{
				new ProductDTO("Test Product 1", 10, 100.00m),
				new ProductDTO("Test Product 2", 20, 200.00m)
			};
			A.CallTo(() => productInterface.GetAllAsync()).Returns(products);
			A.CallTo(() => mapperInterface.Map<IEnumerable<ProductDTO>>(A<IEnumerable<Product>>.Ignored)).Returns(productDTOs);
			// Act
			var result = await productController.GetProducts();
			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnProducts = Assert.IsType<List<ProductDTO>>(okResult.Value);
			Assert.Equal(2, returnProducts.Count);
			Assert.Equal("Test Product 1", returnProducts[0].Name);
			Assert.Equal(100.00m, returnProducts[0].Price);
		}

		[Fact]
		public async Task GetProducts_WhenProductDoesNotExist_ReturnsNotFoundResult()
		{
			// Arrange
			A.CallTo(() => productInterface.GetAllAsync()).Returns(new List<Product>());
			// Act
			var result = await productController.GetProducts();
			// Assert
			var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
			Assert.Equal("Has no any product found.", notFoundResult.Value);
		}

		[Fact]
		public async Task CreateProduct_WhenProductIsNull_ReturnsBadRequestResult()
		{
			// Arrange
			ProductDTO productDTO = null;
			// Act
			var result = await productController.CreateProduct(productDTO);
			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
			Assert.Equal("Product is null.", badRequestResult.Value);
		}

		[Fact] 
		public async Task CreateProduct_WhenModelStateIsInvalid_ReturnsBadRequestResult()
		{
			// Arrange
			var productDTO = new ProductDTO("Test Product", 10, 100.00m);
			productController.ModelState.AddModelError("Name", "Required");
			// Act
			var result = await productController.CreateProduct(productDTO);
			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
			Assert.Equal("The request is invalid.", badRequestResult.Value);
		}

		[Fact]
		public async Task GetProduct_WhenProductExists_ReturnsOkResultWithProduct()
		{
			// Arrange

			var productId = 1;
			var product = new Product
			{
				Id = productId,
				Name = "Test Product",
				Price = 100.00m,
				Quantity = 10
			};

			var productDTO = new ProductDTO
			(
				Name : product.Name,
				Quantity : product.Quantity,
				Price : product.Price
			);

			A.CallTo(() => productInterface.GetByIdAsync(productId)).Returns(product);
			A.CallTo(() => mapperInterface.Map<ProductDTO>(A<Product>.Ignored)).Returns(productDTO);
			// Act
			var result = await productController.GetProduct(productId);
			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnProduct = Assert.IsType<ProductDTO>(okResult.Value);

			Assert.Equal("Test Product", returnProduct.Name);
			Assert.Equal(100.00m, returnProduct.Price);
		}

	}
}
