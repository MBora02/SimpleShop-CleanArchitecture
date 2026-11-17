using SimpleShop.Application.Interfaces.Repositories;
using SimpleShop.Domain.Entities;
using SimpleShop.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
    }
}
