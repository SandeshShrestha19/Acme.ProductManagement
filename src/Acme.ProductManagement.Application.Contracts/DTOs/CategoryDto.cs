using System;
using Acme.ProductManagement.Categories;
using Volo.Abp.Application.Dtos;

namespace Acme.ProductManagement.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CategoryDto()
        {
        }

        public CategoryDto(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
        }
    }
}
