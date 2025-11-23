using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement.Interfaces
{
    public interface IProductService : IApplicationService
    {
        Task CreateAsync(CreateUpdateProductDto createDto);
        Task UpdateAsync(Guid productId, CreateUpdateProductDto updateDto);
        Task DeleteAsync(Guid productId);

        Task<ProductDto> GetAsync(Guid productId);
        Task<List<ProductDto>> GetListAsync();
    }
}
