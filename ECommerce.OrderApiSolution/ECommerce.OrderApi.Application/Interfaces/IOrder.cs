using ECommerce.OrderApi.Domain.Entities;
using ECommerce.SharedLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.Application.Interfaces
{
	public interface IOrder : IGenericInterface<Order>
	{
		Task<IEnumerable<Order>> GetOrdersByAsync(Expression<Func<Order, bool>> predicate);
	}
}
