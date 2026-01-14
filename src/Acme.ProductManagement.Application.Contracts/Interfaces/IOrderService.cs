using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.OrderDto;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Acme.ProductManagement.DTOs.OrdersDto;
using Acme.ProductManagement.Enums;
using Acme.ProductManagement.Orders;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace Acme.ProductManagement.Interfaces
{
    public interface IOrderService : IApplicationService
    {
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(Guid orderId);
        Task<OrderDto> CreateAsync(CreateOrderDto createDto);
        Task AddItemAsync(Guid orderId, AddOrderItemDto input);
        Task RemoveItemAsync(Guid orderId, Guid orderItemId);
        Task DeleteAsync(Guid orderId);
    }
}
