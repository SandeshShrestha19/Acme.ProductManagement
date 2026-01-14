using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductManagement.DTOs.OrdersDto
{
    public class CreateOrderItemInput
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
