using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.OrderItemDto;

namespace Acme.ProductManagement.DTOs.OrderDto
{
    public class CreateOrderDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
    }
}
