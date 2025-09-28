using ECommerce.OrderApi.Application.DTOs;
using ECommerce.OrderApi.Application.DTOs.Conversions;
using ECommerce.OrderApi.Application.Interfaces;
using ECommerce.OrderApi.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.OrderApi.Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OrdersController(IOrder orderInterface, IOrderService orderService) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersAsync()
		{
			var orders = await orderInterface.GetAllAsync();
			if (orders == null || !orders.Any())
			{
				return NotFound("No order detected in database.");
			}
			var (_, list) = OrderConversion.FromEntity(null, orders);
			return Ok(list);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<OrderDTO>> GetOrderByIdAsync(int id)
		{
			var order = await orderInterface.GetByIdAsync(id);
			if (order == null)
			{
				return NotFound("Not found.");
			}
			var (dto, _) = OrderConversion.FromEntity(order, null);
			return Ok(dto);
		}

		[HttpGet("Customer/{clientId}")]
		public async Task<IActionResult> GetOrdersByClientIdAsync(int clientId)
		{
			if (clientId <= 0)
			{
				return BadRequest("Invalid client ID.");
			}
			var orders = await orderService.GetOrdersByClientIdAsync(clientId);
			if (orders == null || !orders.Any())
			{
				return NotFound();
			}
			return Ok(orders);
		}

		[HttpGet("detail/{orderId}")]
		public async Task<IActionResult> GetOrderDetailByOrderIdAsync(int orderId)
		{
			if (orderId <= 0)
			{
				return BadRequest("Invalid order ID.");
			}
			var orderDetail = await orderService.GetOrderDetailByOrderIdAsync(orderId);
			if (orderDetail == null)
			{
				return NotFound();
			}
			return Ok(orderDetail);
		}

		[HttpPost]
		public async Task<ActionResult<OrderDTO>> CreateOrderAsync([FromBody] OrderDTO orderDto)
		{
			if (orderDto == null || !ModelState.IsValid)
			{
				return BadRequest("Invalid order data.");
			}
			var getOrder = orderDto.ToEntity();
			var response = await orderInterface.CreateAsync(getOrder);
			return response.flag ? Ok(response) : BadRequest("Failed to create order.");
		}

		[HttpPut("{id:int}")]
		public async Task<ActionResult<OrderDTO>> UpdateOrderAsync(int id, [FromBody] OrderDTO orderDto)
		{
			if (orderDto == null || !ModelState.IsValid)
			{
				return BadRequest("Invalid order data.");
			}
			var getOrder = orderDto.ToEntity();
			var response = await orderInterface.UpdateAsync(getOrder);
			return response.flag ? Ok(response) : NotFound("Not found.");
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> DeleteOrderAsync(int id)
		{
			var response = await orderInterface.DeleteAsync(id);
			return response.flag ? Ok(response) : NotFound("Not found.");
		}
	}
}
