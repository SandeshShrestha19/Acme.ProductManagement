using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.ProductManagement.Categories;
using Acme.ProductManagement.DTOs;
using Acme.ProductManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;


namespace Acme.ProductManagement.Services
{
    [Authorize]
    public class CategoryService : ApplicationService, ICategoryService, ITransientDependency
    {
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(IRepository<Category, Guid> categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<List<CategoryDto>> GetListAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetListAsync();
                return categories.Select(category => new CategoryDto(category)).ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting list of categories");
                throw;
            }
            
        }
        [HttpGet("category/{categoryId}")]
        public async Task<CategoryDto> GetAsync(Guid categoryId)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(categoryId);
                return new CategoryDto(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting requested category");
                throw;
            }
        }
        [HttpPost]
        public async Task CreateAsync([FromBody] CreateUpdateCategoryDto createDto)
        {
            try
            {
                var category = new Category(name: createDto.Name, description: createDto.Description ?? string.Empty);
                await _categoryRepository.InsertAsync(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category");
                throw;
            }
        }
        [HttpPut("category/{categoryId}")]
        public async Task UpdateAsync(Guid categoryId,[FromBody] CreateUpdateCategoryDto updateDto)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(categoryId);
                category.Update(name: updateDto.Name, description: updateDto.Description ?? string.Empty);
                await _categoryRepository.UpdateAsync(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category");
                throw;
            }
        }
        [HttpDelete("category/{categoryId}")]
        public async Task DeleteAsync(Guid categoryId)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(categoryId);
                await _categoryRepository.DeleteAsync(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category");
                throw;
            }
        }
    }
}
