using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.OrderDto;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Acme.ProductManagement.Enums;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace Acme.ProductManagement.Interfaces
{
    public interface IOrderService : ICrudAppService< 
        OrderDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateOrderDto,
        UpdateOrderDto>
    { 
    }
}
