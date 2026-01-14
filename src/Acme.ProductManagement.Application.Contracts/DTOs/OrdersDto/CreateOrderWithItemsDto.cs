using System;
using System.Collections.Generic;

namespace Acme.ProductManagement.DTOs.OrderDto
{
    public class CreateOrderWithItemsDto
    {
        public Guid CustomerId { get; set; }
        public List<AddOrderItemDto> Items { get; set; } = new List<AddOrderItemDto>();
    }
}
