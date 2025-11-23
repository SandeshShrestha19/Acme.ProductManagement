using System;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.Products;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Acme.ProductManagement.Order
{
    public class OrderItem : Entity<Guid>
    {
        public Guid OrderId { get; set; }
        public Orders.Order Order { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal Total => Product != null ? Quantity * Product.Price : 0;

        protected OrderItem() { }

        public OrderItem(Guid productId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new BusinessException("Quantity must be greater than zero.");
            }
            ProductId = productId;
            Quantity = quantity;
        }
        public OrderItem(Product product, int quantity)
        {
            if (product == null)
            {
                throw new BusinessException("Product cannot be null.");
            }
            if (quantity <= 0)
            {
                throw new BusinessException("Quantity must be greater than zero.");
            }

            ProductId = product.Id;
            Product = product;
            Quantity = quantity;
        }
        public void Update(Guid productId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new BusinessException("Quantity must be greater than zero.");
            }
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
