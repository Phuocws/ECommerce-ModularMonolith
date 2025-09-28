using ECommerce.OrderApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.Application.Services
{
	public interface IOrderService
	{
		Task<IEnumerable<OrderDTO>> GetOrdersByClientIdAsync(int clientId); 
		Task<OrderDetailDTO> GetOrderDetailByOrderIdAsync(int orderId);
	}

}
