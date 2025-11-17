using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            IRepository<Order, Guid> orderRepository,
            IRepository<Customer, Guid> customerRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _customerRepository.GetCountAsync() == 0)
            {
                var customers = new List<Customer>
                {
                    new Customer(id: Guid.NewGuid(),fullName: "John Smith",phoneNumber: "+977-9876561424"),
                    new Customer(id: Guid.NewGuid(),fullName: "Pyarey Shrestha",phoneNumber: "+977-9876456424"),
                    new Customer(id: Guid.NewGuid(),fullName: "Mike Williams",phoneNumber: "+977-984193424"),
                    new Customer(id : Guid.NewGuid(), fullName : "Hati King", phoneNumber : "+977-9876522224"),
                };

                foreach (var customer in customers)
                {
                    await _customerRepository.InsertAsync(customer, autoSave: false);
                }
                await _customerRepository.InsertManyAsync(customers);
            }

            if (await _categoryRepository.GetCountAsync() == 0)
            {
                var liquor = await _categoryRepository.InsertAsync(new Category
                {
                    Name = "Liquor",
                    Description = "Alcohol & Beverages"
                }, autoSave: true);

                var electronics = await _categoryRepository.InsertAsync(new Category
                {
                    Name = "Electronics",
                    Description = "Gadgets & Devices"
                }, autoSave: true);

                if (await _productRepository.GetCountAsync() == 0)
                {
                    var nepalIce = await _productRepository.InsertAsync(new Product
                    {
                        Name = "Nepal Ice",
                        Price = 2.33m,
                        Description = "Nepal's finest beer.",
                        CategoryId = liquor.Id,
                        Stock = 200
                    }, autoSave: true);

                    var samsungS24 = await _productRepository.InsertAsync(new Product
                    {
                        Name = "Samsung S24 Ultra",
                        Price = 1999.99m,
                        Description = "Flagship phone",
                        CategoryId = electronics.Id,
                        Stock = 50
                    }, autoSave: true);

                    var customers = await _customerRepository.GetListAsync();
                    if (customers.Count > 0 && await _orderRepository.GetCountAsync() == 0)
                    {
                        var customer = customers[0]; // Use first customer
                        var order = new Order(Guid.NewGuid(), customer.Id);

                        order.AddItem(nepalIce.Id, 3, nepalIce.Price);
                        order.AddItem(samsungS24.Id, 1, samsungS24.Price);

                        await _orderRepository.InsertAsync(order, autoSave: true);
                    }
                }
            }
        }
    }
}
