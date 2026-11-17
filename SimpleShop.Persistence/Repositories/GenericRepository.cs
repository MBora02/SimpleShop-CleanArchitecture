using Microsoft.EntityFrameworkCore;
using SimpleShop.Application.Interfaces.Repositories;
using SimpleShop.Domain.Common;
using SimpleShop.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }
    }
}
