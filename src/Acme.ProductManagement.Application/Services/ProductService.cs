using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.ProductManagement.DTOs;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    [Authorize]
    public class ProductService : ApplicationService, IProductService, ITransientDependency
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IRepository<Product, Guid> productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<List<ProductDto>> GetListAsync()
        {
            try
            {
                var products = await _productRepository.GetListAsync();
                return products.Select(product => new ProductDto(product)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting list of products");
                throw;
            }

        }
        [HttpGet("product/{productId}")]
        public async Task<ProductDto> GetAsync(Guid productId)
        {
            try
            {
                var product = await _productRepository.GetAsync(productId);
                return new ProductDto(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting requested product");
                throw;
            }
        }
        [HttpPost]
        public async Task CreateAsync(CreateUpdateProductDto createDto)
        {
            try
            {
                var product = new Product(name: createDto.Name,price: createDto.Price, description: createDto.Description, categoryId: createDto.CategoryId);
                await _productRepository.InsertAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                throw;
            }
        }
        [HttpPut("product/{productId}")]
        public async Task UpdateAsync(Guid productId, CreateUpdateProductDto updateDto)
        {
            try
            {
                var product = await _productRepository.GetAsync(productId);
                product.Update(name: updateDto.Name,price: updateDto.Price, description: updateDto.Description, categoryId: updateDto.CategoryId);
                await _productRepository.UpdateAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product");
                throw;
            }
        }
        [HttpDelete("product/{productId}")]
        public async Task DeleteAsync(Guid productId)
        {
            try
            {
                var product = await _productRepository.GetAsync(productId);
                await _productRepository.DeleteAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product");
                throw;
            }
        }
    }
}
