using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.ProductManagement.Categories;
using Acme.ProductManagement.DTOs.CategoriesDto;
using Acme.ProductManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class CategoryService : ApplicationService, ICategoryService, ITransientDependency
    {
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IRepository<Category, Guid> categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                // Use WithDetailsAsync to include Products collection (ABP way)
                var categories = await _categoryRepository.WithDetailsAsync(c => c.Products);
                var categoryList = await AsyncExecuter.ToListAsync(categories);

                if (categoryList == null || !categoryList.Any())
                {
                    _logger.LogWarning("No categories found in the repository.");
                    return new List<CategoryDto>();
                }

                return categoryList.Select(category => new CategoryDto(category)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting list of categories");
                throw new BusinessException(
                    code: "ProductManagement:CategoryListRetrievalError",
                    message: "An error occurred while retrieving categories",
                    innerException: ex
                );
            }
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid categoryId)
        {
            try
            {
                // Use WithDetailsAsync to include Products collection (ABP way)
                var categories = await _categoryRepository.WithDetailsAsync(c => c.Products);
                var category = await AsyncExecuter.FirstOrDefaultAsync(
                    categories.Where(c => c.Id == categoryId));

                if (category == null)
                {
                    _logger.LogWarning("Category not found with ID {CategoryId}", categoryId);
                    throw new BusinessException(
                        code: "ProductManagement:CategoryNotFound",
                        message: $"Category with ID {categoryId} not found"
                    );
                }

                return new CategoryDto(category);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting requested category");
                throw new BusinessException(
                    code: "ProductManagement:CategoryRetrievalError",
                    message: "An error occurred while retrieving the category",
                    innerException: ex
                );
            }
        }

        public async Task CreateAsync([FromBody] CreateCategoryDto createDto)
        {
            try
            {
                var category = new Category(
                    name: createDto.Name,
                    description: createDto.Description ?? string.Empty);

                await _categoryRepository.InsertAsync(category, autoSave: true);

                _logger.LogInformation(
                    "Category created successfully with ID {CategoryId} and Name {CategoryName}",
                    category.Id,
                    category.Name
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category");
                throw new BusinessException(
                    code: "ProductManagement:CategoryCreationError",
                    message: "An error occurred while creating the category",
                    innerException: ex
                );
            }
        }

        public async Task UpdateAsync(Guid categoryId, [FromBody] UpdateCategoryDto updateDto)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(categoryId);

                if (category == null)
                {
                    _logger.LogWarning("Category not found with ID {CategoryId}", categoryId);
                    throw new BusinessException(
                        code: "ProductManagement:CategoryNotFound",
                        message: $"Category with ID {categoryId} not found"
                    );
                }

                category.Update(name: updateDto.Name, description: updateDto.Description);
                await _categoryRepository.UpdateAsync(category, autoSave: true);

                _logger.LogInformation("Category updated successfully with ID {CategoryId}", categoryId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category");
                throw new BusinessException(
                    code: "ProductManagement:CategoryUpdateError",
                    message: "An error occurred while updating the category",
                    innerException: ex
                );
            }
        }

        public async Task DeleteAsync(Guid categoryId)
        {
            try
            {
                // Include Products to check if category has products before deleting
                var categories = await _categoryRepository.WithDetailsAsync(c => c.Products);
                var category = await AsyncExecuter.FirstOrDefaultAsync(
                    categories.Where(c => c.Id == categoryId));

                if (category == null)
                {
                    _logger.LogWarning("Category not found with ID {CategoryId}", categoryId);
                    throw new BusinessException(
                        code: "ProductManagement:CategoryNotFound",
                        message: $"Category with ID {categoryId} not found"
                    );
                }

                // Check if category has products
                if (category.HasProducts())
                {
                    _logger.LogWarning(
                        "Cannot delete category {CategoryId} because it contains {ProductCount} product(s)",
                        categoryId,
                        category.GetProductCount()
                    );
                    throw new BusinessException(
                        code: "ProductManagement:CategoryHasProducts",
                        message: $"Cannot delete category because it contains {category.GetProductCount()} product(s). Please remove or reassign the products first."
                    );
                }

                await _categoryRepository.DeleteAsync(category, autoSave: true);

                _logger.LogInformation("Category deleted successfully with ID {CategoryId}", categoryId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category");
                throw new BusinessException(
                    code: "ProductManagement:CategoryDeletionError",
                    message: "An error occurred while deleting the category",
                    innerException: ex
                );
            }
        }
    }
}