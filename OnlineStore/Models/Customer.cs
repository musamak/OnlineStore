using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ShippingAddressId { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public int BillingAddressId { get; set; }
        public BillingAddress BillingAddress { get; set; }
    }
}
