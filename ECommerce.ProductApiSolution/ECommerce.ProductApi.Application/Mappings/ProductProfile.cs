using AutoMapper;
using ECommerce.ProductApi.Application.DTOs;
using ECommerce.ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ProductApi.Application.Mappings
{
	public class ProductProfile : Profile
	{
		public ProductProfile()
		{
			CreateMap<Product, ProductDTO>().ReverseMap();
		}
	}
}
