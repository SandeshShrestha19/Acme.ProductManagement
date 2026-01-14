using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.DTOs.OrderDto;
using Acme.ProductManagement.DTOs.OrdersDto;
using Acme.ProductManagement.Enums;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.Products;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class OrderService : ApplicationService, IOrderService
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public OrderService(
            IRepository<Order, Guid> orderRepository,
            IRepository<Product, Guid> productRepository,
            IRepository<Customer, Guid> customerRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            try
            {
                var queryable = await _orderRepository.WithDetailsAsync(o => o.Customer, o => o.Items);
                var orders = queryable.ToList();

                if (!orders.Any())
                {
                    Logger.LogWarning("No orders found in the repository.");
                    return new List<OrderDto>();
                }

                return orders.Select(order => new OrderDto(order, includeCustomer: true)).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while retrieving all orders");
                throw;
            }
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                var queryable = await _orderRepository.WithDetailsAsync(o => o.Customer, o => o.Items);
                var order = queryable.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    throw new BusinessException($"Order with ID {orderId} not found.");
                }

                return new OrderDto(order, includeCustomer: true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error occurred while getting order {orderId}");
                throw;
            }
        }

        public async Task<OrderDto> CreateAsync(CreateOrderDto createDto)
        {
            try
            {
                // Validate customer exists
                var customerExists = await _customerRepository.AnyAsync(c => c.Id == createDto.CustomerId);
                if (!customerExists)
                {
                    throw new BusinessException("Customer not found.");
                }

                // Create order
                var order = new Order(createDto.CustomerId);

                // Save order first to get valid ID
                await _orderRepository.InsertAsync(order, autoSave: true);

                // Fetch saved order to ensure ID is populated
                var savedOrder = await _orderRepository.GetAsync(order.Id);

                // Add items if provided
                if (createDto.Items != null && createDto.Items.Any())
                {
                    foreach (var item in createDto.Items)
                    {
                        var product = await _productRepository.GetAsync(item.ProductId);
                        savedOrder.AddItem(product, item.Quantity);
                    }

                    // Update with items
                    await _orderRepository.UpdateAsync(savedOrder, autoSave: true);
                }

                // Reload with all details for DTO
                var queryable = await _orderRepository.WithDetailsAsync(o => o.Customer, o => o.Items);
                var createdOrder = queryable.FirstOrDefault(o => o.Id == savedOrder.Id);

                if (createdOrder == null)
                {
                    throw new BusinessException("Failed to retrieve created order.");
                }

                return new OrderDto(createdOrder, includeCustomer: true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while creating an order");
                throw;
            }
        }

        public async Task AddItemAsync(Guid orderId, AddOrderItemDto input)
        {
            try
            {
                if (input.Quantity <= 0)
                {
                    throw new BusinessException("Quantity must be greater than zero.");
                }

                var queryable = await _orderRepository.WithDetailsAsync(o => o.Items);
                var order = queryable.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    throw new BusinessException($"Order with ID {orderId} not found.");
                }

                var product = await _productRepository.GetAsync(input.ProductId);

                order.AddItem(product, input.Quantity);

                await _orderRepository.UpdateAsync(order, autoSave: true);

                Logger.LogInformation($"Item added to order {orderId} successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error occurred while adding item to order {orderId}");
                throw;
            }
        }

        public async Task RemoveItemAsync(Guid orderId, Guid orderItemId)
        {
            try
            {
                var queryable = await _orderRepository.WithDetailsAsync(o => o.Items);
                var order = queryable.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    throw new BusinessException($"Order with ID {orderId} not found.");
                }

                // Remove item (validation happens in domain logic)
                order.RemoveItem(orderItemId);

                // Save changes
                await _orderRepository.UpdateAsync(order, autoSave: true);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error occurred while removing item from order {orderId}");
                throw;
            }
        }

        public async Task DeleteAsync(Guid orderId)
        {
            try
            {
                var order = await _orderRepository.GetAsync(orderId);

                if (order.OrderStatus != OrderStatus.Pending)
                {
                    throw new BusinessException("Cannot delete a confirmed or completed order.");
                }

                await _orderRepository.DeleteAsync(order, autoSave: true);

                Logger.LogInformation($"Order {orderId} deleted successfully.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Error occurred while deleting order {orderId}");
                throw;
            }
        }
    }
}