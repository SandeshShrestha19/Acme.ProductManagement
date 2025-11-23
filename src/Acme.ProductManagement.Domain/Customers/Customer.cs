using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.ProductManagement.Customers
{
    public class Customer : AuditedAggregateRoot<Guid>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; protected set; }
        public Customer(string fullName, string phoneNumber)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }
        protected Customer() { }

        public void Update(string fullName, string phoneNumber)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }
    }
}
