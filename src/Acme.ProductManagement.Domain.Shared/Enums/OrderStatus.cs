using System;

namespace Acme.ProductManagement.Enums
{
    public enum OrderStatus
    {
        Draft = 0,
        Placed = 1,
        Paid = 2,
        Shipped = 3,
        Completed = 4,
        Cancelled = 5,
        Pending = 6
    }
}
