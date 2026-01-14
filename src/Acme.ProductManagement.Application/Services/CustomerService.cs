using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.DTOs;
using Acme.ProductManagement.DTOs.CustomersDto;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class CustomerService : ApplicationService, ICustomerService, ITransientDependency
    {
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            IRepository<Customer, Guid> customerRepository,
            IRepository<Order, Guid> orderRepository,
            ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            try
            {
                // Use WithDetailsAsync to include Orders and nested OrderItems with Products
                var queryable = await _customerRepository.GetQueryableAsync();
                var customersQuery = queryable
                    .Include(c => c.Orders)
                        .ThenInclude(o => o.Items)
                            .ThenInclude(oi => oi.Product);
                var customerList = await AsyncExecuter.ToListAsync(customersQuery);

                if (customerList == null || !customerList.Any())
                {
                    _logger.LogWarning("No customers found in the repository.");
                    return new List<CustomerDto>();
                }

                return customerList.Select(customer => new CustomerDto(customer)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting list of customers");
                throw new BusinessException(
                    code: "ProductManagement:CustomerListRetrievalError",
                    message: "An error occurred while retrieving customers",
                    innerException: ex
                );
            }
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(Guid customerId)
        {
            try
            {
                // Use WithDetailsAsync to include Orders and nested OrderItems with Products
                var queryable = await _customerRepository.GetQueryableAsync();
                var customer = await AsyncExecuter.FirstOrDefaultAsync(
                    queryable
                        .Include(c => c.Orders)
                            .ThenInclude(o => o.Items)
                                .ThenInclude(oi => oi.Product)
                        .Where(c => c.Id == customerId));

                if (customer == null)
                {
                    _logger.LogWarning("Customer not found with ID {CustomerId}", customerId);
                    throw new BusinessException(
                        code: "ProductManagement:CustomerNotFound",
                        message: $"Customer with ID {customerId} not found"
                    );
                }

                return new CustomerDto(customer);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting info of customer");
                throw new BusinessException(
                    code: "ProductManagement:CustomerRetrievalError",
                    message: "An error occurred while retrieving the customer",
                    innerException: ex
                );
            }
        }

        public async Task CreateAsync(CreateCustomerDto createDto)
        {
            try
            {
                var customer = new Customer(
                    fullName: createDto.Name,
                    phoneNumber: createDto.PhoneNumber);

                await _customerRepository.InsertAsync(customer, autoSave: true);

                _logger.LogInformation(
                    "Customer created successfully with ID {CustomerId} and Name {CustomerName}",
                    customer.Id,
                    customer.FullName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering customer");
                throw new BusinessException(
                    code: "ProductManagement:CustomerCreationError",
                    message: "An error occurred while creating the customer",
                    innerException: ex
                );
            }
        }

        public async Task DeleteAsync(Guid customerId)
        {
            try
            {
                // Include Orders to check if customer has orders before deleting
                var customers = await _customerRepository.WithDetailsAsync(c => c.Orders);
                var customer = await AsyncExecuter.FirstOrDefaultAsync(
                    customers.Where(c => c.Id == customerId));

                if (customer == null)
                {
                    _logger.LogWarning("Customer not found with ID {CustomerId}", customerId);
                    throw new BusinessException(
                        code: "ProductManagement:CustomerNotFound",
                        message: $"Customer with ID {customerId} not found"
                    );
                }

                // Optional: Check if customer has orders before deletion
                if (customer.Orders != null && customer.Orders.Any())
                {
                    _logger.LogWarning(
                        "Cannot delete customer {CustomerId} because they have {OrderCount} order(s)",
                        customerId,
                        customer.Orders.Count
                    );
                    throw new BusinessException(
                        code: "ProductManagement:CustomerHasOrders",
                        message: $"Cannot delete customer because they have {customer.Orders.Count} order(s). Please remove the orders first."
                    );
                }

                await _customerRepository.DeleteAsync(customer, autoSave: true);

                _logger.LogInformation("Customer deleted successfully with ID {CustomerId}", customerId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting customer details");
                throw new BusinessException(
                    code: "ProductManagement:CustomerDeletionError",
                    message: "An error occurred while deleting the customer",
                    innerException: ex
                );
            }
        }

        public async Task UpdateAsync(Guid customerId, UpdateCustomerDto updateDto)
        {
            try
            {
                var customer = await _customerRepository.GetAsync(customerId);

                if (customer == null)
                {
                    _logger.LogWarning("Customer not found with ID {CustomerId}", customerId);
                    throw new BusinessException(
                        code: "ProductManagement:CustomerNotFound",
                        message: $"Customer with ID {customerId} not found"
                    );
                }

                customer.Update(fullName: updateDto.Name, phoneNumber: updateDto.PhoneNumber);
                await _customerRepository.UpdateAsync(customer, autoSave: true);

                _logger.LogInformation("Customer updated successfully with ID {CustomerId}", customerId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating customer details");
                throw new BusinessException(
                    code: "ProductManagement:CustomerUpdateError",
                    message: "An error occurred while updating the customer",
                    innerException: ex
                );
            }
        }
    }
}