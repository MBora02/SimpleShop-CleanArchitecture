using SimpleShop.Application.Common;
using SimpleShop.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<Result<IReadOnlyList<ProductResponseDto>>> GetAllAsync();
        Task<Result<ProductResponseDto>> GetByIdAsync(int id);
        Task<Result<ProductResponseDto>> CreateAsync(ProductCreateDto dto);
        Task<Result> UpdateAsync(int id, ProductUpdateDto dto);
        Task<Result> DeleteAsync(int id);
    }
}
