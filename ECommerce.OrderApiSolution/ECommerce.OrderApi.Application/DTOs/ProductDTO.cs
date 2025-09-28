using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.Application.DTOs
{
	public record ProductDTO
	(
		[Required]
		string Name,
		[Required, Range(1, int.MaxValue)]
		int Quantity,
		[Required, DataType(DataType.Currency)]
		decimal Price
		);
}
