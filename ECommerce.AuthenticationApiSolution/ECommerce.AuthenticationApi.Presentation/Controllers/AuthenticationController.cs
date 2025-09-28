using ECommerce.AuthenticationApi.Application.DTOs;
using ECommerce.AuthenticationApi.Application.Interfaces;
using ECommerce.SharedLibrary.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.AuthenticationApi.Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class AuthenticationController(IUser userInterface) : ControllerBase
	{
		[HttpPost]
		public async Task<ActionResult<Response>> Register([FromBody] AppUserDTO appUserDTO)
		{
			if (appUserDTO == null || !ModelState.IsValid)
			{
				return BadRequest("Invalid user data.");
			}
			var response = await userInterface.Register(appUserDTO);
			return (response.flag) ? Ok(response) : BadRequest(response);
		}

		[HttpPost("login")]
		public async Task<ActionResult<Response>> Login([FromBody] LoginDTO loginDTO)
		{
			if (loginDTO == null || !ModelState.IsValid)
			{
				return BadRequest("Invalid login data.");
			}
			var response = await userInterface.Login(loginDTO);
			return (response.flag) ? Ok(response) : BadRequest(response);
		}

		[HttpGet("{id}")]
		[Authorize]
		public async Task<ActionResult<GetUserDTO>> GetUserById(int id)
		{
			if (id <= 0)
			{
				return BadRequest("Invalid user ID.");
			}
			var user = await userInterface.GetUserById(id);
			if (user == null)
			{
				return NotFound("User not found.");
			}
			return Ok(user);
		}
	}
}
