using System.ComponentModel.DataAnnotations;

namespace ECommerce.AuthenticationApi.Application.DTOs
{
	public record AppUserDTO
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
		string Password,
		[Required]
		string Role
	);

}
