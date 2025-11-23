using System;
using System.Collections.Generic;
using Acme.ProductManagement.DTOs.OrderItemDto;

namespace Acme.ProductManagement.DTOs.OrderDto
{
    public class UpdateOrderDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public List<UpdateOrderItemDto> OrderItems { get; set; } = new();
    }
}
