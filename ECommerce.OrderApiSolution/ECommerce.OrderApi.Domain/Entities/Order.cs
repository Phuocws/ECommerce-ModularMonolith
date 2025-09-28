using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.OrderApi.Domain.Entities
{
	public class Order
	{
		public int Id { get; set; }
		public int CustomerId { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.UtcNow;
		public int ProductId { get; set; }
		public int PurchaseQuantity { get; set; }
	}
}
