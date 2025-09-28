using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.AuthenticationApi.Application.DTOs
{
	public record LoginDTO
	(
		[Required, EmailAddress]
		string Email,
		[Required]
		string Password
	);

}
