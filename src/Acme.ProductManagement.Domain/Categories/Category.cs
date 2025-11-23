using System;
using System.Collections.Generic;
using Acme.ProductManagement.Products;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.ProductManagement.Categories
{
    public class Category : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();

        protected Category() { }
        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
