using System;
using Acme.ProductManagement.Categories;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.ProductManagement.Products
{
    public class Product : AuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int CurrentStock { get; private set; } = 0;
        public Guid CategoryId { get; private set; }

        public Category Category { get; private set; }

        protected Product() { }

        public Product(string name, decimal price, string description, int currentStock, Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name is required.", nameof(name));
            }

            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
            }

            if (currentStock < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentStock), "Stock cannot be negative.");
            }

            Name = name;
            Price = price;
            CurrentStock = currentStock;
            Description = description;

            SetCategory(category);
        }

        internal Product(Guid id, string name, decimal price, string description, int currentStock, Guid categoryId) : base(id) {
            Name = name;
            Price = price;
            CurrentStock = currentStock;
            Description = description;
            CategoryId = categoryId;
        }

        public void Update(string? name, decimal? price, string? description)
        {
            if (name != null)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Product name cannot be empty.", nameof(name));
                }
                Name = name;
            }

            if (price.HasValue)
            {
                if (price.Value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
                }
                Price = price.Value;
            }

            if (description != null)
            {
                Description = description;
            }
        }

        public void ChangeCategory(Category newCategory)
        {
            if (newCategory == null)
            {
                throw new ArgumentNullException(nameof(newCategory), "Category cannot be null.");
            }

            if (CategoryId == newCategory.Id)
            {
                return; // Already in this category
            }

            Category?.RemoveProduct(this);

            // Set new category
            SetCategory(newCategory);
        }

        private void SetCategory(Category category)
        {
            Category = category;
            CategoryId = category.Id;

            category.AddProduct(this);
        }

        public void IncreaseStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
            }
            CurrentStock += quantity;
        }

        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Reducing quantity must be greater than zero.");
            }

            if (CurrentStock < quantity)
            {
                throw new InvalidOperationException($"Insufficient stock. Available: {CurrentStock}, Requested: {quantity}");
            }

            CurrentStock -= quantity;
        }
    }
}