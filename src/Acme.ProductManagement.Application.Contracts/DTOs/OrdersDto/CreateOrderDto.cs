using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Acme.ProductManagement.OrderItems;

namespace Acme.ProductManagement.DTOs.OrderDto
{
    public class CreateOrderDto
    {
        [Required]
        public Guid CustomerId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }
}
