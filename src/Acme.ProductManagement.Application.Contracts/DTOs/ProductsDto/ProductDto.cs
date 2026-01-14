using System;
using Acme.ProductManagement.Products;
using Volo.Abp.Application.Dtos;

namespace Acme.ProductManagement.DTOs.ProductsDto
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int CurrentStock { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }

        public ProductDto(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            CurrentStock = product.CurrentStock;
            Description = product.Description;
            CategoryId = product.CategoryId;
        }
    }
}
