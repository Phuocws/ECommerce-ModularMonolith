using ECommerce.AuthenticationApi.Application.DTOs;
using ECommerce.SharedLibrary.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.AuthenticationApi.Application.Interfaces
{
	public interface IUser
	{
		Task<Response> Register(AppUserDTO appUserDTO);
		Task<Response> Login(LoginDTO loginDTO);
		Task<GetUserDTO> GetUserById(int id);
	}
}
