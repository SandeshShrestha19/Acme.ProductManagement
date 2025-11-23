using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.ProductManagement.Products;
using Volo.Abp.Domain.Entities;

namespace Acme.ProductManagement.Inventories
{
    public class Inventory : Entity<Guid>
    {
        public Guid ProductId { get; set; }
        public int CurrentStock { get; set; } = 0;

        public Product Product { get; set; }

        protected Inventory() { }

        public Inventory(Guid productId, int currentStock)
        {
            ProductId = productId;
            CurrentStock = currentStock;
        }

        public void IncreaseStock(int quantity)
        {
            if(quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "quantity must be greater than zero.");
            }
            CurrentStock += quantity;
        }
        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), "Reducing quantity must be greater than zero.");
            }
            CurrentStock -= quantity;
        }
    }
}
