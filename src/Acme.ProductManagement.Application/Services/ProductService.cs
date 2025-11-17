using System;
using Acme.ProductManagement.DTOs;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.Products;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace Acme.ProductManagement.Services
{
    public class ProductService : CrudAppService<
        Product, 
        ProductDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateProductDto>, IProductService
    {
        public ProductService(IRepository<Product, Guid> productRepository) : base(productRepository) { }
    }

        


    
}
