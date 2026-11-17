using SimpleShop.Application.Interfaces.Repositories;
using SimpleShop.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IProductRepository? _productRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IProductRepository Products =>
            _productRepository ??= new ProductRepository(_context);
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
