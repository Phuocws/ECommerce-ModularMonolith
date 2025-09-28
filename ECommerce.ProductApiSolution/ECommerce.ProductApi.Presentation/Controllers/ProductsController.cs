using AutoMapper;
using ECommerce.ProductApi.Application.DTOs;
using ECommerce.ProductApi.Application.Interfaces;
using ECommerce.ProductApi.Domain.Entities;
using ECommerce.SharedLibrary.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.ProductApi.Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[AllowAnonymous]
	public class ProductsController(IProduct productRepository, IMapper mapper) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
		{
			var products = await productRepository.GetAllAsync();
			if (products == null || !products.Any())
			{
				return NotFound("Has no any product found.");
			}
			else
			{
				return Ok(mapper.Map<IEnumerable<ProductDTO>>(products));
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ProductDTO>> GetProduct(int id)
		{
			var product = await productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound("Product not found.");
			}
			else
			{
				return Ok(mapper.Map<ProductDTO>(product));
			}
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<Response>> CreateProduct(ProductDTO productDTO)
		{
			if (productDTO == null)
			{
				return BadRequest("Product is null.");
			}
			else if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			else
			{
				var product = mapper.Map<Product>(productDTO);
				var response = await productRepository.CreateAsync(product);
				return response.flag
					? CreatedAtAction(nameof(GetProduct), new { id = product.Id }, response)
					: BadRequest(response);
			}
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<Response>> UpdateProduct(int id, ProductDTO productDTO)
		{
			if (productDTO == null)
			{
				return BadRequest("Product is null.");
			}
			else if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			else
			{
				var product = await productRepository.GetByIdAsync(id);
				if (product == null)
					return NotFound("Product not found.");

				mapper.Map(productDTO, product);
				var response = await productRepository.UpdateAsync(product);
				return response.flag
					? Ok(response)
					: BadRequest(response);
			}
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<Response>> DeleteProduct(int id)
		{
			var product = await productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound("Product not found.");
			}
			var response = await productRepository.DeleteAsync(id);
			return response.flag
				? Ok(response)
				: BadRequest(response);
		}
	}
}
