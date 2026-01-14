using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs.CategoriesDto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement.Interfaces
{
    public interface ICategoryService : IApplicationService
    {
        Task CreateAsync(CreateCategoryDto createDto);
        Task UpdateAsync(Guid categoryId, UpdateCategoryDto updateDto);
        Task DeleteAsync(Guid categoryId);

        Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
    }
}
