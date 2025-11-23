using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement.Interfaces
{
    public interface ICategoryService : IApplicationService
    {
        Task CreateAsync(CreateUpdateCategoryDto createDto);
        Task UpdateAsync(Guid categoryId, CreateUpdateCategoryDto upadteDto);
        Task DeleteAsync(Guid categoryId);

        Task<CategoryDto> GetAsync(Guid categoryId);
        Task<List<CategoryDto>> GetListAsync();
    }
}
