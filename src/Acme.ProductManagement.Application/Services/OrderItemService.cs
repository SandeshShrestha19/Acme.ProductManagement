using System;
using Acme.ProductManagement.DTOs.ItemsDto;
using Acme.ProductManagement.DTOs.OrderDto;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.OrderItems;
using Acme.ProductManagement.Orders;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class OrderItemService : CrudAppService<
        OrderItem,
        OrderItemDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateOrderItemDto,
        UpdateOrderItemDto>, IOrderItemService
    {
        public OrderItemService(IRepository<OrderItem, Guid> orderItemRepository)
        : base(orderItemRepository)
        {

        }
    }
}
