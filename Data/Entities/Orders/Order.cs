using System;
using System.Collections.Generic;
namespace ECommerce.Data.Entities.Orders
{
    public class Orderss
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Placed { get; set; } = DateTime.UtcNow;
        public List<OrderItems> Items { get; set; } = new List<OrderItems>();
        public Addresss DeliveryAddress { get; set; }
        public PaymentStatus PaymentStatus { get; set; } =
        PaymentStatus.Pending;
        public AppUser User { get; set; }
    }
}