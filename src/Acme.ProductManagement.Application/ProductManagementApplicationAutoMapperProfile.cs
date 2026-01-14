using Acme.ProductManagement.Categories;
using Acme.ProductManagement.Customers;
using Acme.ProductManagement.DTOs.CategoriesDto;
using Acme.ProductManagement.DTOs.CustomersDto;
using Acme.ProductManagement.DTOs.ItemsDto;
using Acme.ProductManagement.DTOs.OrderDto;
using Acme.ProductManagement.DTOs.OrderItemDto;
using Acme.ProductManagement.DTOs.OrdersDto;
using Acme.ProductManagement.DTOs.ProductsDto;
using Acme.ProductManagement.OrderItems;
using Acme.ProductManagement.Orders;
using Acme.ProductManagement.Products;
using AutoMapper;

namespace Acme.ProductManagement;

public class ProductManagementApplicationAutoMapperProfile : Profile
{
    public ProductManagementApplicationAutoMapperProfile()
    {
        // Product mappings
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>(); // Fixed: was UpdateCustomerDto

        // Category mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CustomerId,
                opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer != null ? src.Customer.FullName : string.Empty))
            .ForMember(dest => dest.CustomerPhone,
                opt => opt.MapFrom(src => src.Customer != null ? src.Customer.PhoneNumber : string.Empty))
            .ForMember(dest => dest.OrderDate,
                opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderStatus,
                opt => opt.MapFrom(src => src.OrderStatus))
            .ForMember(dest => dest.TotalAmount,
                opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.OrderItems,
                opt => opt.MapFrom(src => src.Items))
            .PreserveReferences();

        CreateMap<CreateOrderDto, Order>()
            .ForMember(dest => dest.Customer, opt => opt.Ignore()) 
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
            .ForMember(dest => dest.OrderStatus, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore()); 

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.ProductName ?? (src.Product != null ? src.Product.Name : "Unknown Product")))
            .ForMember(dest => dest.Quantity,
                opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.TotalPrice,
                opt => opt.MapFrom(src => src.TotalPrice))
            .PreserveReferences();

        CreateMap<CreateOrderItemDto, OrderItem>()
            .ForMember(dest => dest.Product, opt => opt.Ignore()) 
            .ForMember(dest => dest.Order, opt => opt.Ignore()) 
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateOrderItemDto, OrderItem>()
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        // Customer mappings
        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.Orders, opt => opt.Ignore()) 
            .PreserveReferences();

        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore()); 

        CreateMap<UpdateCustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore());
    }
}