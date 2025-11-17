using System;
using Acme.ProductManagement.DTOs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement.Interfaces
{
    public interface ICategoryService : ICrudAppService< //Defines CRUD methods
        CategoryDto, 
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCategoryDto>
    {
    }
}
