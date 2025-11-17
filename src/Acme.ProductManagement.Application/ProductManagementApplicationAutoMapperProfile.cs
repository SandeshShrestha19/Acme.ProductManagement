using Acme.ProductManagement.Categories;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.DTOs;
using Acme.ProductManagement.DTOs.CustomersDto;
using Acme.ProductManagement.DTOs.ItemsDto;
using Acme.ProductManagement.DTOs.OrderDto;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Acme.ProductManagement.OrderItems;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.Products;
using AutoMapper;

namespace Acme.ProductManagement;

public class ProductManagementApplicationAutoMapperProfile : Profile
{
    public ProductManagementApplicationAutoMapperProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();

        CreateMap<Category, CategoryDto>();
        CreateMap<CreateUpdateCategoryDto, Category>();

        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<UpdateOrderItemDto, OrderItem>();
        CreateMap<CreateOrderDto, Order>();

        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>();
    }
}
