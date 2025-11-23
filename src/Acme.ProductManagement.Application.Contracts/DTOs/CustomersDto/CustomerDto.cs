using System;
using Acme.ProductManagement.Customers;
using Volo.Abp.Application.Dtos;

namespace Acme.ProductManagement.DTOs.CustomersDto
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public CustomerDto()
        {
        }

        public CustomerDto(Customer customer)
        {
            Id = customer.Id;
            Name = customer.FullName;
            PhoneNumber = customer.PhoneNumber;
        }
    }
}
