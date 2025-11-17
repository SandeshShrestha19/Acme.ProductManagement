using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.DTOs.OrderDto;
using Acme.ProductManagement.Enums;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.OrderItems;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.Products;
using AutoMapper.Internal.Mappers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class OrderService : CrudAppService<
        Order, 
        OrderDto,
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateOrderDto,
        UpdateOrderDto>, IOrderService
    {
        public OrderService(IRepository<Order, Guid> orderRepository)
        : base(orderRepository)
        {

        }
    }
}
