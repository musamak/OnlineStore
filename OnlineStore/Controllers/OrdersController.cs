using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDBContext _context;

        public OrdersController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<coOrderInfo>>> GetOrders()
        {
            List<Order> lstOrder = await _context.Orders.ToListAsync();
            List<coOrderInfo> lstOrderInfo = new List<coOrderInfo>();

            foreach(Order ord in lstOrder)
            {
                Customer customer = _context.Customers.Where(r => r.CustomerId == ord.CustomerId).SingleOrDefault();
                lstOrderInfo.Add(new coOrderInfo
                {
                    OrderId = ord.OrderId,
                    CustomerName = string.Concat(new string[] { customer.FirstName, " ", customer.LastName }),
                    OrderDate = ord.OrderDate
                });
            }
            return lstOrderInfo; 
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            Order order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            order.TrackingNumber = string.IsNullOrEmpty(order.TrackingNumber) ? "" : order.TrackingNumber;

            Customer customer = _context.Customers.Where(r => r.CustomerId == order.CustomerId).SingleOrDefault();
            customer.ShippingAddress = _context.ShippingAddresss.Where(r => r.AddressId == customer.ShippingAddressId).SingleOrDefault();
            customer.BillingAddress = _context.BillingAddresss.Where(r => r.AddressId == customer.BillingAddressId).SingleOrDefault();
            order.Customer = customer;

            List<OrderDetail> lstOrderDetail = await _context.OrderDetails.Where(r => r.OrderId == order.OrderId).ToListAsync();

            foreach (OrderDetail ordDet in lstOrderDetail)
            {
                Product product = _context.Products.Where(r => r.ProductId == ordDet.ProductId).SingleOrDefault();
                ordDet.Product = product;
            }
            order.OrderDetails = lstOrderDetail;

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/{trackingnumber}")]
        public async Task<IActionResult> PutOrder(int id, string trackingnumber)
        {
            Order order = await _context.Orders.FindAsync(id);

            order.TrackingNumber = trackingnumber;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(trackingnumber);
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
