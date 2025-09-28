using ECommerce.OrderApi.Application.DTOs;
using ECommerce.OrderApi.Application.Interfaces;
using ECommerce.OrderApi.Application.Services;
using ECommerce.OrderApi.Domain.Entities;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.UnitTest.Services
{
	public class OrderServiceTest
	{
		private readonly IOrderService _orderService;
		private readonly IOrder _orderInterface;

		public OrderServiceTest()
		{
			_orderInterface = A.Fake<IOrder>();
			_orderService = A.Fake<IOrderService>();
		}
		public class FakeHttpMessageHandler(HttpResponseMessage respone) : HttpMessageHandler
		{
			private readonly HttpResponseMessage _response = respone;

			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			{
				return Task.FromResult(_response);
			}
		}

		private static HttpClient CreateFakeHttpClient(object o)
		{
			var handler = new HttpResponseMessage
			{
				StatusCode = System.Net.HttpStatusCode.OK,
				Content = JsonContent.Create(o)
			};
			var fakeHandler = new FakeHttpMessageHandler(handler);
			var client = new HttpClient(fakeHandler)
			{
				BaseAddress = new Uri("http://localhost")
			};
			return client;
		}

		[Fact]
		public async Task GetProduct_ValidProductId_ReturnsProduct()
		{
			// Arrange
			var productId = 1;
			var expectedProduct = new ProductDTO(Name: "Test Product", Quantity: 10, Price: 10.00m);
			var fakeHttpClient = CreateFakeHttpClient(expectedProduct);

			var _orderService = new OrderService(null!, fakeHttpClient, null!);

			// Act
			var result = await _orderService.GetProductByIdAsync(productId);
			// Assert
			Assert.NotNull(result);
			Assert.Equal(expectedProduct, result);
		}

		[Fact]
		public async Task GetOrderByClientId_ValidClientId_ReturnsOrders()
		{
			// Arrange
			var clientId = 1;
			var expectedOrders = new List<Order>
			{
				new Order{Id = 1, CustomerId = clientId, ProductId = 1, PurchaseQuantity = 2, OrderDate = DateTime.UtcNow},
				new Order{ Id = 2, CustomerId = clientId, ProductId = 2, PurchaseQuantity = 3, OrderDate = DateTime.UtcNow }
			};
			var fakeHttpClient = CreateFakeHttpClient(expectedOrders);
			var _orderService = new OrderService(_orderInterface, fakeHttpClient, null!);
			A.CallTo(() => _orderInterface.GetOrdersByAsync(A<Expression<Func<Order, bool>>>.Ignored))
				.Returns(expectedOrders);
			// Act
			var result = await _orderService.GetOrdersByClientIdAsync(clientId);
			// Assert
			Assert.NotNull(result);
			Assert.Equal(expectedOrders.Count, result.Count());
		}
	}
}
