using ECommerce.OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.Application.DTOs.Conversions
{
	public static class OrderConversion
	{
		public static Order ToEntity(this OrderDTO orderDTO)
		{
			return new Order
			{
				Id = orderDTO.Id,
				ProductId = orderDTO.ProductId,
				CustomerId = orderDTO.CustomerId,
				PurchaseQuantity = orderDTO.PurchaseQuantity,
				OrderDate = orderDTO.OrderDate
			};
		}
		public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(this Order? order, IEnumerable<Order>? orders)
		{
			if (order is not null || orders is null)
			{
				var singleOrder = new OrderDTO
				(
					order!.Id,
					order.CustomerId,
					order.ProductId,
					order.PurchaseQuantity,
					order.OrderDate
				);
				return (singleOrder, null);
			}
			else
			{
				var orderList = orders!.Select(o => new OrderDTO
				(
					o.Id,
					o.CustomerId,
					o.ProductId,
					o.PurchaseQuantity,
					o.OrderDate
				));
				return (null, orderList);
			}
		}
	}
}
