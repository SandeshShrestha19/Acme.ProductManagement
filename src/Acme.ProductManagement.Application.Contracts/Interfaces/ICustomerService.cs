using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.CustomersDto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement.Interfaces
{
    public interface ICustomerService : IApplicationService
    {
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task CreateAsync(CreateCustomerDto createDto);
        Task UpdateAsync(Guid customerId, UpdateCustomerDto updateDto);
        Task DeleteAsync(Guid customerId);
        Task<CustomerDto> GetCustomerByIdAsync(Guid customerId);
    }
}
