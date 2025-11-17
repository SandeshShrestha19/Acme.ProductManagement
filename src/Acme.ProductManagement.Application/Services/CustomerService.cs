using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.DTOs.CustomersDto;
using Acme.ProductManagement.Interfaces;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class CustomerService : CrudAppService<Customer, CustomerDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCustomerDto>, ICustomerService
    {
        public CustomerService(IRepository<Customer, Guid> customerRepository) : base(customerRepository) { }
    }
}
