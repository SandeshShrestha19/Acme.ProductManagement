using Acme.ProductManagement.Categories;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.DTOs;
using Acme.ProductManagement.DTOs.CustomersDto;
using Acme.ProductManagement.DTOs.ItemsDto;
using Acme.ProductManagement.DTOs.OrderDto;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Acme.ProductManagement.Order;
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

        CreateMap<Orders.Order, OrderDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.FullName : string.Empty));
        CreateMap<OrderItem, DTOs.ItemsDto.OrderItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0));
        CreateMap<DTOs.OrderItemDto.CreateOrderItemDto, OrderItem>();
        CreateMap<DTOs.OrderItemDto.UpdateOrderItemDto, OrderItem>();
        CreateMap<CreateOrderDto, Orders.Order>();

        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>();
    }
}
