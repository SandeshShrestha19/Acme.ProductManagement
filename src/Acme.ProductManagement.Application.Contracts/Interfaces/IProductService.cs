using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.ProductsDto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement.Interfaces
{
    public interface IProductService : IApplicationService
    {
        Task CreateAsync(CreateProductDto createDto);
        Task UpdateAsync(Guid productId, UpdateProductDto updateDto);
        Task DeleteAsync(Guid productId);
        Task<ProductDto> GetProductByIdAsync(Guid productId);
        Task<List<ProductDto>> GetAllProductsAsync();
    }
}
