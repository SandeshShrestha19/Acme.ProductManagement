using System;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.Inventory;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.Inventories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class InventoryService : ApplicationService, IInventoryService, ITransientDependency
    {
        private readonly IRepository<Inventory, Guid> _inventoryRepository;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IRepository<Inventory, Guid> inventoryRepository, ILogger<InventoryService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _logger = logger;
        }
        [HttpPut("increase-stock/{productId}")]
        public async Task IncreaseStockAsync(Guid productId, UpdateInventoryDto increaseDto)
        {
            try
            {
                var inventory = await _inventoryRepository.FirstOrDefaultAsync(x => x.ProductId == productId);
                if (inventory == null)
                {
                    throw new InvalidOperationException($"Inventory not found for product {productId}");
                }
                inventory.IncreaseStock(increaseDto.StockQuantity);
                await _inventoryRepository.UpdateAsync(inventory);

                _logger.LogInformation("Stock increased for product {ProductId} by {Quantity}",
                    productId, increaseDto.StockQuantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while increasing stock for product {ProductId}", productId);
                throw;
            }
        }
        [HttpPut("decrease-stock/{productId}")]
        public async Task DecreaseStockAsync(Guid productId, UpdateInventoryDto decreaseDto)
        {
            try
            {
                var inventory = await _inventoryRepository.FirstOrDefaultAsync(x => x.ProductId == productId);
                if (inventory == null)
                {
                    throw new InvalidOperationException($"Inventory not found for product {productId}");
                }
                inventory.ReduceStock(decreaseDto.StockQuantity);
                await _inventoryRepository.UpdateAsync(inventory);

                _logger.LogInformation("Stock decreased for product {ProductId} by {Quantity}",
                    productId, decreaseDto.StockQuantity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while decreasing stock for product {ProductId}", productId);
                throw;
            }
        }
    }
}
