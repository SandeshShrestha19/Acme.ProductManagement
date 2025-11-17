using System;
using Acme.ProductManagement.DTOs;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Acme.ProductManagement.Interfaces
{
    public interface IProductService : ICrudAppService< 
        ProductDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateProductDto>
    {

    }
    //you can inherit from IApplicationService and create your own methods.
}
