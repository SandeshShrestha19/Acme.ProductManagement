using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.ItemsDto;
using Acme.ProductManagement.DTOs.OrderDto;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace Acme.ProductManagement.Interfaces
{
    public interface IOrderItemService : ICrudAppService<
        OrderItemDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateOrderItemDto,
        UpdateOrderItemDto>
    {
    }
}
