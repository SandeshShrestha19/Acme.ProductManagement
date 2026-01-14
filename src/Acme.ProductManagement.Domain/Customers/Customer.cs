using System;
using System.Collections.Generic;
using Acme.ProductManagement.Orders;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.ProductManagement.Customers
{
    public class Customer : AuditedAggregateRoot<Guid>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        protected Customer() { }

        public Customer(string fullName, string phoneNumber)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }

        public void Update(string fullName, string phoneNumber)
        {
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                FullName = fullName;
            }

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                PhoneNumber = phoneNumber;
            }
        }
    }
}