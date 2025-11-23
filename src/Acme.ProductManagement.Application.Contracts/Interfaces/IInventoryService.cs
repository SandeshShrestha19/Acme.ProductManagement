using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.Inventory;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement.Interfaces
{
    public interface IInventoryService : IApplicationService
    {
        Task IncreaseStockAsync(Guid productId, UpdateInventoryDto increaseDto);
        Task DecreaseStockAsync(Guid productId, UpdateInventoryDto decreaseDto);
    }
}
