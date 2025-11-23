using System;
using System.Collections.Generic;
using System.Linq;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.Enums;
using Acme.ProductManagement.Order;
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
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal TotalAmount => OrderItems?.Sum(item => item.Total) ?? 0;

        protected Order() { }

        public Order(Guid customerId)
        {
            CustomerId = customerId;
            OrderDate = DateTime.Now;
            OrderStatus = OrderStatus.Pending;
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

            var existingItem = OrderItems.FirstOrDefault(orderItem => orderItem.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                OrderItems.Add(new OrderItem(product.Id, quantity));
            }
        }
    }
}
