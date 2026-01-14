using System;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.OrderItems;
using Acme.ProductManagement.Products;
using System.Net.Sockets;

namespace Acme.ProductManagement.DTOs.ItemsDto
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderItemDto()
        {
        }

        public OrderItemDto(OrderItem orderItem)
        {
            Id = orderItem.Id;
            ProductName = orderItem.ProductName;
            Price = orderItem.UnitPrice;  
            Quantity = orderItem.Quantity;
            TotalPrice = orderItem.TotalPrice;
        }
    }

}
