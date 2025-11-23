using System;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.CustomersDto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement.Interfaces
{
    public interface ICustomerService : IApplicationService
    {
        Task CreateAsync(CreateUpdateCustomerDto createDto);
        Task UpdateAsync(Guid customerId, CreateUpdateCustomerDto updateDto);
        Task DeleteAsync(Guid customerId);
        Task<CustomerDto> GetAsync(Guid customerId);
    }
}
