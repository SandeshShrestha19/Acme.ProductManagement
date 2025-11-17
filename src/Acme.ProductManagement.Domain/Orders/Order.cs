using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.Enums;
using Acme.ProductManagement.OrderItems;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace Acme.ProductManagement.Orders
{
    public class Order : AuditedAggregateRoot<Guid>
    {
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } 

        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public decimal TotalAmount => OrderItems.Sum(item => item.UnitPrice * item.Quantity);

        protected Order() { } 

        public Order(Guid id, Guid customerId) : base(id)
        {
            CustomerId = customerId;
            OrderDate = DateTime.Now;
            OrderStatus = OrderStatus.Pending;
        }

        public void AddItem(Guid productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0)
            {
                throw new BusinessException("Quantity must be greater than zero.");
            }
            OrderItems.Add(new OrderItem(Guid.NewGuid(), Id, productId, quantity, unitPrice));
        }
    }
}
