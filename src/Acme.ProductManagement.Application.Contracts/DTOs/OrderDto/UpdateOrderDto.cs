using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Acme.ProductManagement.Enums;

namespace Acme.ProductManagement.DTOs.OrderDto
{
    public class UpdateOrderDto
    {
        public OrderStatus OrderStatus { get; set; }

        public List<UpdateOrderItemDto> OrderItems { get; set; } = new();
    }
}
