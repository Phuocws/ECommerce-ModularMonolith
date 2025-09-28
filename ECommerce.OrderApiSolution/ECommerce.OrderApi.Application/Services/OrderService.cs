using ECommerce.OrderApi.Application.DTOs;
using ECommerce.OrderApi.Application.DTOs.Conversions;
using ECommerce.OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;
using System.Net.Http.Json;

namespace ECommerce.OrderApi.Application.Services
{
	public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipelineProvider) : IOrderService
	{
		public async Task<ProductDTO> GetProductByIdAsync(int productId)
		{
			var getProduct = await httpClient.GetAsync($"/api/Products/{productId}");
			if (!getProduct.IsSuccessStatusCode)
			{
				return null!;
			}
			else
			{
				var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
				return product!;
			}
		}

		public async Task<AppUserDTO> GetUserByIdAsync(int userId)
		{
			var getUser = await httpClient.GetAsync($"api/authentication/{userId}");
			if (!getUser.IsSuccessStatusCode)
			{
				return null!;
			}
			else
			{
				var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
				return user!;
			}
		}

		public async Task<OrderDetailDTO> GetOrderDetailByOrderIdAsync(int orderId)
		{
			var order = await orderInterface.GetByIdAsync(orderId);
			if (order == null)
			{
				return null!;
			}

			var retryPipeline = resiliencePipelineProvider.GetPipeline("retry");

			var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProductByIdAsync(order.ProductId));
			var userDTO = await retryPipeline.ExecuteAsync(async token => await GetUserByIdAsync(order.CustomerId));

			return new OrderDetailDTO
			(
				order.Id,
				order.CustomerId,
				userDTO.Name,
				userDTO.Email,
				userDTO.Address,
				userDTO.PhoneNumber,
				productDTO.Name,
				order.PurchaseQuantity,
				productDTO.Price,
				productDTO.Price * order.PurchaseQuantity,
				order.OrderDate
			);
		}

		public async Task<IEnumerable<OrderDTO>> GetOrdersByClientIdAsync(int clientId)
		{
			var orders = await orderInterface.GetOrdersByAsync(o => o.CustomerId == clientId);
			if (!orders.Any())
			{
				return null!;
			}
			else
			{
				var (_, ordersResult) = OrderConversion.FromEntity(null, orders); 
				return ordersResult!;
			}
		}
	}
}
