using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.Application.DTOs
{
	public record AppUserDTO
	(
		[Required]
		string Name,
		[Required, EmailAddress]
		string Email,
		[Required]
		string PhoneNumber,
		[Required]
		string Address,
		[Required] 
		string Password,
		[Required]
		string Role
	);
}
