using System;
using System.Collections.Generic;
using System.Linq;
using Acme.ProductManagement.DTOs.ItemsDto;
using Acme.ProductManagement.Enums;
using Acme.ProductManagement.Orders;

namespace Acme.ProductManagement.DTOs.OrdersDto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ItemsDto.OrderItemDto> OrderItems { get; set; } = new List<ItemsDto.OrderItemDto>();

        public OrderDto()
        {
        }

        public OrderDto(Order order, bool includeCustomer = true)
        {
            Id = order.Id;
            CustomerId = order.CustomerId;
            OrderDate = order.OrderDate;
            OrderStatus = order.OrderStatus;
            TotalAmount = order.TotalAmount;

            if (includeCustomer && order.Customer != null)
            {
                CustomerName = order.Customer.FullName;
                CustomerPhone = order.Customer.PhoneNumber;
            }
            else
            {
                CustomerName = string.Empty;
                CustomerPhone = string.Empty;
            }

            OrderItems = order.Items?.Select(item => new ItemsDto.OrderItemDto(item)).ToList()
                         ?? new List<ItemsDto.OrderItemDto>();
        }
    }
}