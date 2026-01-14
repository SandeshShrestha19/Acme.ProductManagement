using System;
using System.Collections.Generic;
using System.Linq;
using Acme.ProductManagement.Products;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.ProductManagement.Categories
{
    public class Category : AuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        private readonly List<Product> _products = new List<Product>();
        public IReadOnlyList<Product> Products => _products.AsReadOnly();

        protected Category() { }

        public Category(string name, string description = null)
        {
            Name = name;
            Description = description;
        }

        public void Update(string name, string description = null)
        {
            Name = name;
            Description = description;
        }

        internal void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                return;
            }

            _products.Add(product);
        }

        internal void RemoveProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            _products.Remove(product);
        }

        // Public method to remove product by Id (for application service layer)
        public void RemoveProductById(Guid productId)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                _products.Remove(product);
            }
        }

        public bool HasProducts()
        {
            return _products.Any();
        }

        public int GetProductCount()
        {
            return _products.Count;
        }

        public bool ContainsProduct(Guid productId)
        {
            return _products.Any(p => p.Id == productId);
        }
    }
}