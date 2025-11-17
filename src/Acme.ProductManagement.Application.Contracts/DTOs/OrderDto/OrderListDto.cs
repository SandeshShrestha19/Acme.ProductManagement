using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.Enums;

namespace Acme.ProductManagement.DTOs.OrderDto
{
    public class OrderListDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string OrderStatusDisplay => OrderStatus.ToString();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }
}
