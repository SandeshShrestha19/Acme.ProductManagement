using System;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.Products;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Acme.ProductManagement.OrderItems
{
    public class OrderItem : Entity<Guid>
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } 

        protected OrderItem() { }

        public OrderItem(Guid orderId, Product product, int quantity)
        {
            ValidateProduct(product);
            ValidateQuantity(quantity);

            OrderId = orderId; 
            ProductId = product.Id;
            ProductName = product.Name;
            Product = product;
            Quantity = quantity;
            UnitPrice = product.Price; 
            TotalPrice = UnitPrice * Quantity; 
        }

        public OrderItem(Guid orderId, Guid productId, string productName, int quantity, decimal unitPrice)
        {
            if (productId == Guid.Empty)
            {
                throw new BusinessException("ProductId cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new BusinessException("Product name cannot be empty.");
            }
            ValidateQuantity(quantity);

            OrderId = orderId; 
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        private void ValidateProduct(Product product)
        {
            if (product == null)
            {
                throw new BusinessException("Product cannot be null.");
            }
        }

        private void ValidateQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new BusinessException("Quantity must be greater than zero.");
            }
        }

        public void IncreaseQuantity(int increaseQuantity)
        {
            if (increaseQuantity <= 0)
            {
                throw new BusinessException("Increase quantity must be greater than zero.");
            }
            Quantity += increaseQuantity;
        }

        public void DecreaseQuantity(int decreaseQuantity)
        {
            if (decreaseQuantity <= 0)
            {
                throw new BusinessException("Decrease quantity must be greater than zero.");
            }
            if (Quantity <= decreaseQuantity)
            {
                throw new BusinessException("Decrease quantity exceeds current quantity.");
            }
            Quantity -= decreaseQuantity;
        }
    }
}