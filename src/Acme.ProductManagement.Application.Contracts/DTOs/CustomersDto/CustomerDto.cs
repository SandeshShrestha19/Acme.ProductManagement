using System;
using System.Collections.Generic;
using System.Linq;
using Acme.ProductManagement.Customers;

namespace Acme.ProductManagement.DTOs.CustomersDto
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public List<OrdersDto.OrderDto> Orders { get; set; } = new List<OrdersDto.OrderDto>();

        public CustomerDto()
        {
        }

        public CustomerDto(Customer customer)
        {
            Id = customer.Id;
            FullName = customer.FullName;
            PhoneNumber = customer.PhoneNumber;
            Orders = customer.Orders?.Select(order => new OrdersDto.OrderDto(order)).ToList()
                     ?? new List<OrdersDto.OrderDto>();
        }
    }
}