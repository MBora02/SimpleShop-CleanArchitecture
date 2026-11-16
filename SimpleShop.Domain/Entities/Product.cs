using SimpleShop.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
