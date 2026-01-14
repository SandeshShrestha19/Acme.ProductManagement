using System;
using System.Collections.Generic;
using System.Linq;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.Enums;
using Acme.ProductManagement.OrderItems;
using Acme.ProductManagement.Products;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.ProductManagement.Orders
{
    public class Order : AuditedAggregateRoot<Guid>
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalAmount => Items?.Sum(item => item.TotalPrice) ?? 0;

        protected Order() { }

        public Order(Guid customerId)
        {
            CustomerId = customerId;
            OrderDate = DateTime.UtcNow;
            OrderStatus = OrderStatus.Pending;
            Items = new List<OrderItem>();
        }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
            {
                throw new BusinessException("Quantity must be greater than zero.");
            }
            if (product == null)
            {
                throw new BusinessException("Product cannot be null.");
            }

            var existingItem = Items.FirstOrDefault(orderItem => orderItem.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity); // Use the existing method
            }
            else
            {
                // FIX: Pass this.Id as the first parameter
                var orderItem = new OrderItem(this.Id, product, quantity);
                Items.Add(orderItem);
            }
        }

        public void RemoveItem(Guid orderItemId)
        {
            if (OrderStatus != OrderStatus.Pending)
            {
                throw new BusinessException("Cannot modify a confirmed order.");
            }
            var item = Items.FirstOrDefault(x => x.Id == orderItemId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        //public decimal GetTotalWithDiscount(decimal discountPercentage)
        //{
        //    if (discountPercentage < 0 || discountPercentage > 100)
        //    {
        //        throw new BusinessException("Discount percentage must be between 0 and 100.");
        //    }

        //    var discount = TotalAmount * (discountPercentage / 100);
        //    return TotalAmount - discount;
        //}
    }
}
