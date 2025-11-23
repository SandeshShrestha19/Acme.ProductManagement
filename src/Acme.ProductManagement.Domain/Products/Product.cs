using System;
using Acme.ProductManagement.Categories;
using Acme.ProductManagement.Inventories;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.ProductManagement.Products
{
    public class Product : AuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }
        public Inventory Inventory { get; private set; }
        protected Product() { }

        public Product(string name, decimal price, string description, Guid categoryId)
        {
            Name = name;
            Price = price;
            Description = description;
            CategoryId = categoryId;
        }
        public void Update(string name, decimal price, string description, Guid categoryId)
        {
            Name = name;
            Price = price;
            Description = description;
            CategoryId = categoryId;
        }
    }
}
