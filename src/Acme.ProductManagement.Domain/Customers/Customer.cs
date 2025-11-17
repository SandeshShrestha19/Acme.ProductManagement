using System;
using System.Xml.Linq;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.ProductManagement.Customers
{
    public class Customer : AuditedAggregateRoot<Guid>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public Customer(Guid id, string fullName, string phoneNumber) : base(id)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }

        protected Customer() { }
    }
}
