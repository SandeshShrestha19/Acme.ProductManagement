using System;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    [Authorize]
    public class OrderItemService : ApplicationService, IOrderItemService, ITransientDependency
    {
        private readonly IRepository<OrderItem, Guid> _orderItemRepository;
        private readonly ILogger<OrderItemService> _logger;

        public OrderItemService(IRepository<OrderItem, Guid> orderItemRepository, ILogger<OrderItemService> logger)
        {
            _orderItemRepository = orderItemRepository;
            _logger = logger;
        }
        [HttpPost]
        public async Task CreateAsync(CreateOrderItemDto createDto)
        {
            try
            {
                var orderItem = new OrderItem(productId: createDto.ProductId, quantity: createDto.Quantity);
                await _orderItemRepository.InsertAsync(orderItem);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while ordering item");
                throw;
            }
        }
        [HttpPut("order-item/{itemId}")]
        public async Task UpdateAsync(Guid itemId, UpdateOrderItemDto updateDto)
        {
            try
            {
                var orderItem = await _orderItemRepository.GetAsync(itemId);
                orderItem.Update(updateDto.ProductId, updateDto.Quantity);
                await _orderItemRepository.UpdateAsync(orderItem);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while updating item");
                throw;
            }
        }
        [HttpDelete("order-item/{itemId}")]
        public async Task DeleteAsync(Guid itemId)
        {
            try
            {
                var orderItem = await _orderItemRepository.GetAsync(itemId);
                await _orderItemRepository.DeleteAsync(orderItem);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error occurred while deleting order item");
                throw;
            }

        }
    }
}
