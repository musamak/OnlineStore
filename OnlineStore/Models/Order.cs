using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal ShippingPrice { get; set; }
        public string TrackingNumber { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Customer Customer { get; set; }

    }
}
