using System;
using System.Collections.Generic;
using Acme.ProductManagement.Enums;

namespace Acme.ProductManagement.DTOs.OrderDto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ItemsDto.OrderItemDto> OrderItems { get; set; } = new();

        public OrderDto()
        {
        }
    }
}
