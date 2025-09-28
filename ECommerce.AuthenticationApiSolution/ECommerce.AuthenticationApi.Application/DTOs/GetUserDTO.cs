using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.AuthenticationApi.Application.DTOs
{
	public record GetUserDTO
	(
		int Id,
		[Required]
		string Name,
		[Required, EmailAddress]
		string Email,
		[Required]
		string PhoneNumber,
		[Required]
		string Address,
		[Required]
		string Role
	);
}
