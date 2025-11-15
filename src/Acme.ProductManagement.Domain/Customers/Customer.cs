using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.ProductManagement.Customers
{
    public class Customer : AuditedAggregateRoot<Guid>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
