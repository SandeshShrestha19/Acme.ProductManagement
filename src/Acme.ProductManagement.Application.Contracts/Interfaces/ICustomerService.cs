using System;
using Acme.ProductManagement.DTOs.CustomersDto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement.Interfaces
{
    public interface ICustomerService : ICrudAppService<CustomerDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCustomerDto>
    {
    }
}
