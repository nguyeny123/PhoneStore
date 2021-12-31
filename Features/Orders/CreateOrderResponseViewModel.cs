using ECommerce.Data.Entities;
using ECommerce.Data.Entities.Orders;

namespace ECommerce.Features.Orders
{
  	public class CreateOrderResponseViewModel
	{
		public int OrderId { get; set; }
		public string PaymentStatus { get; set; }

		public CreateOrderResponseViewModel(int orderId, PaymentStatus paymentStatus)
		{
			OrderId = orderId;
			PaymentStatus = paymentStatus.ToString();
		}
	}
}