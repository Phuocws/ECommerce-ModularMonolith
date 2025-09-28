using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.Application.DTOs
{
	public record OrderDetailDTO
	(
		[Required]
		int OrderId,
		[Required]
		int CustomerId,
		[Required]
		string CustomerName,
		[Required, EmailAddress]
		string CustomerEmail,
		[Required]
		string CustomerAddress,
		[Required, Phone]
		string CustomerPhone,
		[Required]
		string ProductName,
		[Required]
		int PurchaseQuantity,
		[Required, DataType(DataType.Currency)]
		decimal ProductPrice,
		[Required, DataType(DataType.Currency)]
		decimal TotalPrice,
		[Required]
		DateTime OrderDate
		);
}
