using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductManagement.DTOs.OrderItemDto
{
    public class UpdateOrderItemDto
    {
        public int Quantity {  get; set; }
        public decimal UnitPrice { get; set; }
    }
}
