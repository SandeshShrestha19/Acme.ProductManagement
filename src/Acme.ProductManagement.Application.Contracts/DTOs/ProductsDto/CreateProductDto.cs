using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductManagement.DTOs.ProductsDto
{
    public class CreateProductDto
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CurrentStock { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public Guid CategoryId { get; set; }
    }
}
