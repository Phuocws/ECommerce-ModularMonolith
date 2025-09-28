using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.Application.DTOs
{
	public record OrderDTO
	(
		int Id,
		[Required, Range(1, int.MaxValue)]
		int CustomerId,
		[Required, Range(1, int.MaxValue)]
		int ProductId,
		[Required, Range(1, int.MaxValue)]
		int PurchaseQuantity,
		DateTime OrderDate
	);
}
