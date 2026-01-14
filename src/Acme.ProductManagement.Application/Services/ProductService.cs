using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.ProductManagement.Categories;
using Acme.ProductManagement.DTOs.Inventory;
using Acme.ProductManagement.DTOs.ProductsDto;
using Acme.ProductManagement.Interfaces;
using Acme.ProductManagement.Products;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement.Services
{
    public class ProductService : ApplicationService, IProductService, ITransientDependency
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IRepository<Product, Guid> productRepository,
            IRepository<Category, Guid> categoryRepository,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetListAsync();

                if (products == null || products.Count == 0)
                {
                    _logger.LogInformation("No products found");
                    return new List<ProductDto>();
                }

                return products.Select(product => new ProductDto(product)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting list of products");
                throw new BusinessException(
                    code: "ProductManagement:ProductListRetrievalError",
                    message: "An error occurred while retrieving products",
                    innerException: ex
                );
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid productId)
        {
            try
            {
                var product = await _productRepository.GetAsync(productId);

                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID {ProductId}", productId);
                    throw new BusinessException(
                        code: "ProductManagement:ProductNotFound",
                        message: $"Product with ID {productId} not found"
                    );
                }

                return new ProductDto(product);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting product {ProductId}", productId);
                throw new BusinessException(
                    code: "ProductManagement:ProductRetrievalError",
                    message: "An error occurred while retrieving the product",
                    innerException: ex
                );
            }
        }

        public async Task CreateAsync(CreateProductDto createDto)
        {
            try
            {
                // Get the category - this will throw if not found
                var category = await _categoryRepository.GetAsync(createDto.CategoryId);

                // Create product with category object
                // This automatically adds the product to the category's collection
                var product = new Product(
                    name: createDto.Name,
                    price: createDto.Price,
                    description: createDto.Description,
                    currentStock: createDto.CurrentStock,
                    category: category  // Pass category object instead of categoryId
                );

                await _productRepository.InsertAsync(product, autoSave: true);

                // Update the category to persist the relationship
                // The product is already added to category's collection in the constructor
                await _categoryRepository.UpdateAsync(category, autoSave: true);

                _logger.LogInformation(
                    "Product created successfully with ID {ProductId} and Name {ProductName}",
                    product.Id,
                    product.Name
                );
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                throw new BusinessException(
                    code: "ProductManagement:ProductCreationError",
                    message: "An error occurred while creating the product",
                    innerException: ex
                );
            }
        }

        public async Task UpdateAsync(Guid productId, UpdateProductDto updateDto)
        {
            try
            {
                var product = await _productRepository.GetAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID {ProductId}", productId);
                    throw new BusinessException(
                        code: "ProductManagement:ProductNotFound",
                        message: $"Product with ID {productId} not found"
                    );
                }

                var categoryChanged = updateDto.CategoryId.HasValue && updateDto.CategoryId.Value != product.CategoryId;

                if (categoryChanged)
                {
                    // Get the new category
                    var newCategory = await _categoryRepository.GetAsync(updateDto.CategoryId.Value);

                    // Get the old category
                    var oldCategory = await _categoryRepository.GetAsync(product.CategoryId);

                    // Change the category - this manages the bidirectional relationship
                    product.ChangeCategory(newCategory);

                    // Update product properties (without categoryId since it's handled by ChangeCategory)
                    product.Update(
                        name: updateDto.Name,
                        price: updateDto.Price,
                        description: updateDto.Description
                    );

                    // Save changes
                    await _productRepository.UpdateAsync(product, autoSave: true);
                    await _categoryRepository.UpdateAsync(oldCategory, autoSave: true);
                    await _categoryRepository.UpdateAsync(newCategory, autoSave: true);

                    _logger.LogInformation(
                        "Product {ProductId} updated and moved from category {OldCategoryId} to {NewCategoryId}",
                        productId, oldCategory.Id, newCategory.Id
                    );
                }
                else
                {
                    // No category change, just update the product properties
                    product.Update(
                        name: updateDto.Name,
                        price: updateDto.Price,
                        description: updateDto.Description
                    );

                    await _productRepository.UpdateAsync(product, autoSave: true);

                    _logger.LogInformation("Product updated successfully with ID {ProductId}", productId);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product {ProductId}", productId);
                throw new BusinessException(
                    code: "ProductManagement:ProductUpdateError",
                    message: "An error occurred while updating the product",
                    innerException: ex
                );
            }
        }

        public async Task DeleteAsync(Guid productId)
        {
            try
            {
                var product = await _productRepository.GetAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID {ProductId}", productId);
                    throw new BusinessException(
                        code: "ProductManagement:ProductNotFound",
                        message: $"Product with ID {productId} not found"
                    );
                }

                // Remove product from category
                var category = await _categoryRepository.GetAsync(product.CategoryId);
                category.RemoveProductById(productId);

                await _categoryRepository.UpdateAsync(category, autoSave: true);

                // Delete the product
                await _productRepository.DeleteAsync(productId, autoSave: true);

                _logger.LogInformation("Product deleted successfully with ID {ProductId}", productId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product {ProductId}", productId);
                throw new BusinessException(
                    code: "ProductManagement:ProductDeletionError",
                    message: "An error occurred while deleting the product",
                    innerException: ex
                );
            }
        }

        public async Task IncreaseStockAsync(Guid productId, UpdateStockDto increaseDto)
        {
            try
            {
                var product = await _productRepository.FirstOrDefaultAsync(x => x.Id == productId);

                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID {ProductId}", productId);
                    throw new BusinessException(
                        code: "ProductManagement:ProductNotFound",
                        message: $"Product with ID {productId} not found"
                    );
                }

                product.IncreaseStock(increaseDto.Quantity);

                await _productRepository.UpdateAsync(product, autoSave: true);

                _logger.LogInformation(
                    "Stock increased for product {ProductId} by {Quantity}. New stock: {NewStock}",
                    productId,
                    increaseDto.Quantity,
                    product.CurrentStock
                );
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex, "Invalid quantity for stock increase: {ProductId}", productId);
                throw new BusinessException(
                    code: "ProductManagement:InvalidQuantity",
                    message: ex.Message,
                    innerException: ex
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while increasing stock for product {ProductId}", productId);
                throw new BusinessException(
                    code: "ProductManagement:StockIncreaseError",
                    message: "An error occurred while increasing stock",
                    innerException: ex
                );
            }
        }

        public async Task DecreaseStockAsync(Guid productId, UpdateStockDto decreaseDto)
        {
            try
            {
                var product = await _productRepository.FirstOrDefaultAsync(x => x.Id == productId);

                if (product == null)
                {
                    _logger.LogWarning("Product not found with ID {ProductId}", productId);
                    throw new BusinessException(
                        code: "ProductManagement:ProductNotFound",
                        message: $"Product with ID {productId} not found"
                    );
                }

                product.ReduceStock(decreaseDto.Quantity);

                await _productRepository.UpdateAsync(product, autoSave: true);

                _logger.LogInformation(
                    "Stock decreased for product {ProductId} by {Quantity}. Remaining stock: {RemainingStock}",
                    productId,
                    decreaseDto.Quantity,
                    product.CurrentStock
                );
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogWarning(ex, "Invalid quantity for stock decrease: {ProductId}", productId);
                throw new BusinessException(
                    code: "ProductManagement:InvalidQuantity",
                    message: ex.Message,
                    innerException: ex
                );
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Insufficient stock for product {ProductId}", productId);
                throw new BusinessException(
                    code: "ProductManagement:InsufficientStock",
                    message: ex.Message,
                    innerException: ex
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while decreasing stock for product {ProductId}", productId);
                throw new BusinessException(
                    code: "ProductManagement:StockDecreaseError",
                    message: "An error occurred while decreasing stock",
                    innerException: ex
                );
            }
        }
    }
}