using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.DTOs.ItemsDto;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.OrderItems;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class OrderItemService : ApplicationService, IOrderItemService, ITransientDependency
    {
        private readonly IRepository<OrderItem, Guid> _orderItemRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly ILogger<OrderItemService> _logger;

        public OrderItemService(IRepository<OrderItem, Guid> orderItemRepository, ILogger<OrderItemService> logger, IRepository<Product, Guid> productRepository)
        {
            _orderItemRepository = orderItemRepository;
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<List<OrderItemDto>> GetAllOrderItemsAsync()
        {
            try
            {
                var orderItems = await _orderItemRepository.WithDetailsAsync(x => x.Product);

                if (orderItems == null || !orderItems.Any())
                {
                    _logger.LogWarning("No order items found in the repository.");
                    return new List<OrderItemDto>();
                }
                return orderItems.Select(orderItem => new OrderItemDto(orderItem)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting list of order items");
                throw;
            }
        }

        //public async Task CreateAsync(CreateOrderItemDto createDto)
        //{
        //    try
        //    {
        //        var product = await _productRepository.FirstOrDefaultAsync(product => product.Id == createDto.ProductId);

        //        if (product == null)
        //        {
        //            throw new BusinessException("Product not found.");
        //        }

        //        var orderItem = new OrderItem(product, createDto.Quantity);
        //        await _orderItemRepository.InsertAsync(orderItem);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while creating order item");
        //        throw;
        //    }
        //}


        //public async Task UpdateAsync(Guid itemId, UpdateOrderItemDto updateDto)
        //{
        //    try
        //    {
        //        var orderItem = await _orderItemRepository.GetAsync(itemId);
        //        orderItem.Update(updateDto.ProductId, updateDto.Quantity);
        //        await _orderItemRepository.UpdateAsync(orderItem);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, "Error occurred while updating item");
        //        throw;
        //    }
        //}
        //public async Task DeleteAsync(Guid itemId)
        //{
        //    try
        //    {
        //        var orderItem = await _orderItemRepository.GetAsync(itemId);
        //        await _orderItemRepository.DeleteAsync(orderItem);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, "Error occurred while deleting order item");
        //        throw;
        //    }

        //}
    }
}
