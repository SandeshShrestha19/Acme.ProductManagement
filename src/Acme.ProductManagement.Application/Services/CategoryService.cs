using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.Categories;
using Acme.ProductManagement.DTOs;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.Products;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class CategoryService : CrudAppService<Category, CategoryDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCategoryDto>, ICategoryService
    {
        public CategoryService(IRepository<Category, Guid> categoryRepository) : base(categoryRepository) { }
    }
}
