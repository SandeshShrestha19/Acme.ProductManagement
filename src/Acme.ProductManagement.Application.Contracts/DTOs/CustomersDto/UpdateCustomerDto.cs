using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductManagement.DTOs.CustomersDto
{
    public class UpdateCustomerDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }
    }
}
