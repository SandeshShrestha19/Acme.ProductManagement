using System;
using System.Threading.Tasks;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.DTOs.OrderDto;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.Order;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class OrderService : ApplicationService, IOrderService, ITransientDependency
    {
        private readonly IRepository<Orders.Order, Guid> _orderRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IRepository<Orders.Order, Guid> orderRepository, IRepository<Product, Guid> productRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
        }
        [HttpPost]
        public async Task CreateAsync(CreateOrderDto createDto)
        {
            try
            {
                var order = new Orders.Order(customerId: createDto.CustomerId);
                await _orderRepository.InsertAsync(order);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while creating a order");
                throw;
            }
        }
        [HttpPut("order/{orderId}")]
        public async Task UpdateAsync(Guid orderId, UpdateOrderDto updateDto)
        {
            try
            {
                var order = await _orderRepository.GetAsync(orderId);
                var product = await _productRepository.GetAsync(updateDto.ProductId);
                order.AddItem(product: product, quantity: updateDto.Quantity);
                await _orderRepository.UpdateAsync(order);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while updating order");
                throw;
            }
        }
        [HttpDelete("order/{orderId}")]
        public async Task DeleteAsync(Guid orderId)
        {
            try
            {
                var order = await _orderRepository.GetAsync(orderId);
                await _orderRepository.DeleteAsync(order);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while deleting order");
                throw;
            }

        }
    }
}
