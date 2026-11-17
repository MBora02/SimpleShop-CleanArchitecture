using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        // Yönettiği repository'ler
        IProductRepository Products { get; }
        // Tüm değişiklikleri tek seferde veritabanına kaydeder
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
