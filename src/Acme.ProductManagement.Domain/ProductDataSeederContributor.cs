using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.ProductManagement.Categories;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.Inventories;
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
        private readonly IRepository<Orders.Order, Guid> _orderRepository;
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<Inventory, Guid> _inventoryRepository;

        public ProductDataSeederContributor(
            IRepository<Product, Guid> productRepository,
            IRepository<Category, Guid> categoryRepository,
            IRepository<Orders.Order, Guid> orderRepository,
            IRepository<Customer, Guid> customerRepository,
            IRepository<Inventory, Guid> inventoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
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

            Category liquor = null;
            Category electronics = null;

            if (await _categoryRepository.GetCountAsync() == 0)
            {
                liquor = await _categoryRepository.InsertAsync(
                    new Category("Liquor", "Alcohol & Beverages"), true);

                electronics = await _categoryRepository.InsertAsync(
                    new Category("Electronics", "Gadgets & Devices"), true);
            }
            else
            {
                var categories = await _categoryRepository.GetListAsync();
                liquor = categories.Find(c => c.Name == "Liquor");
                electronics = categories.Find(c => c.Name == "Electronics");
            }

            Product nepalIce = null;
            Product samsungS24 = null;

            if (await _productRepository.GetCountAsync() == 0)
            {
                nepalIce = await _productRepository.InsertAsync(
                    new Product("NepalIce", 2.33m, "Nepal's finest beer.", liquor.Id),
                    true);

                await _inventoryRepository.InsertAsync(
                    new Inventory(nepalIce.Id, 100),
                    true);

                samsungS24 = await _productRepository.InsertAsync(
                    new Product("Samsung S24 Ultra", 1999.99m, "Flagship phone", electronics.Id),
                    true);

                await _inventoryRepository.InsertAsync(
                    new Inventory(samsungS24.Id, 50),
                    true);
            }

            if (await _orderRepository.GetCountAsync() == 0)
            {
                var customers = await _customerRepository.GetListAsync();
                if (customers.Count > 0 && nepalIce != null)
                {
                    var customer = customers[0];

                    var order = new Orders.Order(customer.Id);
                    order.AddItem(nepalIce, 3);
                    order.AddItem(samsungS24, 1);

                    await _orderRepository.InsertAsync(order, true);
                }
            }
        }
    }
}
