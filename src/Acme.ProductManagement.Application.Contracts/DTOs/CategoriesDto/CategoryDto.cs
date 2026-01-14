using System;
using System.Collections.Generic;
using System.Linq;
using Acme.ProductManagement.Categories;
using Acme.ProductManagement.DTOs.ProductsDto;
using Acme.ProductManagement.Orders;
using Volo.Abp.Application.Dtos;

namespace Acme.ProductManagement.DTOs.CategoriesDto
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        protected CategoryDto()
        {
        }

        public CategoryDto(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
            Products = category.Products?.Select(product => new ProductDto(product)).ToList() ?? new List<ProductDto>();
        }
    }
}
