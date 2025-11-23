using System;
using System.Threading.Tasks;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.DTOs.CustomersDto;
using Acme.ProductManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    [Authorize]
    public class CustomerService : ApplicationService, ICustomerService, ITransientDependency
    {
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(IRepository<Customer, Guid> customerRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }
        [HttpGet("customer/{customerId}")]
        public async Task<CustomerDto> GetAsync(Guid customerId)
        {
            try
            {
                var customer = await _customerRepository.GetAsync(customerId);
                return new CustomerDto(customer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while getting info of customer");
                throw;
            }
        }
        [HttpPost]
        public async Task CreateAsync(CreateUpdateCustomerDto createDto)
        {
            try
            {
                var customer = new Customer(fullName: createDto.Name, phoneNumber: createDto.PhoneNumber);
                await _customerRepository.InsertAsync(customer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while registering customer");
                throw;
            }

        }
        [HttpDelete("customer/{customerId}")]
        public async Task DeleteAsync(Guid customerId)
        {
            try
            {
                var customer = await _customerRepository.GetAsync(customerId);
                await _customerRepository.DeleteAsync(customer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while deleting customer details");
                throw;
            }

        }
        [HttpPut("customer/{customerId}")]
        public async Task UpdateAsync(Guid customerId, CreateUpdateCustomerDto updateDto)
        {
            try
            {
                var customer = await _customerRepository.GetAsync(customerId);
                customer.Update(fullName: updateDto.Name, phoneNumber: updateDto.PhoneNumber);
                await _customerRepository.UpdateAsync(customer);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while updating customer details");
                throw;
            }

        }
    }
}
