using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.ProductManagement.DTOs.CategoriesDto
{
    public class UpdateCategoryDto
    {
        [StringLength(128)]
        public string? Name { get; set; } = string.Empty;

        [StringLength(1028)]
        public string? Description { get; set; }
    }
}
