using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.ProductManagement.Categories;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.Products;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.ProductManagement
{
    public class ProductDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;

        public ProductDataSeederContributor(
            IRepository<Product, Guid> productRepository,
            IRepository<Category, Guid> categoryRepository,
            IRepository<Orders.Order, Guid> orderRepository,
            IRepository<Customer, Guid> customerRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // Seed Customers
            if (await _customerRepository.GetCountAsync() == 0)
            {
                await _customerRepository.InsertManyAsync(new List<Customer>
                {
                    new Customer("John Smith", "+977-9876561424"),
                    new Customer("Pyarey Shrestha", "+977-9876456424"),
                    new Customer("Mike Williams", "+977-984193424"),
                    new Customer("Hati King", "+977-9876522224")
                }, autoSave: true);
            }

            // Seed Categories
            Category liquor = null;
            Category electronics = null;

            if (await _categoryRepository.GetCountAsync() == 0)
            {
                liquor = await _categoryRepository.InsertAsync(
                    new Category("Liquor", "Alcohol & Beverages"), autoSave: true);
                electronics = await _categoryRepository.InsertAsync(
                    new Category("Electronics", "Gadgets & Devices"), autoSave: true);
            }
            else
            {
                var categories = await _categoryRepository.GetListAsync();
                liquor = categories.Find(c => c.Name == "Liquor");
                electronics = categories.Find(c => c.Name == "Electronics");
            }

            // Seed Products
            if (await _productRepository.GetCountAsync() == 0)
            {
                await _productRepository.InsertAsync(
                    new Product("NepalIce", 2.33m, "Nepal's finest beer.", 10, category: liquor),
                    autoSave: true);

                await _productRepository.InsertAsync(
                    new Product("Samsung S24 Ultra", 1999.99m, "Flagship phone", 20, category: electronics),
                    autoSave: true);

                await _productRepository.InsertAsync(
                    new Product("iPhone 15 Pro", 1299.99m, "Apple flagship", 15, category: electronics),
                    autoSave: true);

                await _categoryRepository.UpdateAsync(liquor, autoSave: true);
                await _categoryRepository.UpdateAsync(electronics, autoSave: true);
            }

            if (await _orderRepository.GetCountAsync() == 0)
            {
                var customers = await _customerRepository.GetListAsync();
                if (customers.Count > 0)
                {
                    await _orderRepository.InsertAsync(new Order(customers[0].Id), autoSave: true);
                    await _orderRepository.InsertAsync(new Order(customers[1].Id), autoSave: true);
                    await _orderRepository.InsertAsync(new Order(customers[2].Id), autoSave: true);
                }
            }
        }
    }
}