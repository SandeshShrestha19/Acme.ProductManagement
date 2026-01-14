using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductManagement.DTOs.ProductsDto
{
    public class UpdateProductDto
    {
        [MaxLength(128)]
        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public Guid? CategoryId { get; set; }
    }
}

