using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using SimpleShop.Application.Common;
using SimpleShop.Application.DTOs.Product;
using SimpleShop.Application.Interfaces.Repositories;
using SimpleShop.Application.Interfaces.Services;
using SimpleShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ProductCreateDto> _createValidator;
        private readonly IValidator<ProductUpdateDto> _updateValidator;
        private readonly IMemoryCache _cache;

        private const string ProductsCacheKey = "products_cache_list";

        public ProductService(
            IUnitOfWork unitOfWork,
            IValidator<ProductCreateDto> createValidator,
            IValidator<ProductUpdateDto> updateValidator,
            IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _cache = cache;
        }
        public async Task<Result<IReadOnlyList<ProductResponseDto>>> GetAllAsync()
        {
            // 1. Önce Cache'e bak: RAM'de bu liste var mı?
            if (_cache.TryGetValue(ProductsCacheKey, out IReadOnlyList<ProductResponseDto>? cachedProducts))
            {
                return Result<IReadOnlyList<ProductResponseDto>>.Success(cachedProducts!, "Ürünler ÖNBELLEKTEN (RAM) getirildi.");
            }
            // 2. Cache'te yoksa Veritabanına (SQL) git:
            var products = await _unitOfWork.Products.GetAllAsync();
            var responseDtos = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CreatedDate = p.CreatedDate
            }).ToList();
            // 3. Veritabanından gelen veriyi 5 dakikalığına RAM'e kaydet:
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));  
            _cache.Set(ProductsCacheKey, responseDtos, cacheOptions);
            return Result<IReadOnlyList<ProductResponseDto>>.Success(responseDtos, "Ürünler VERİTABANINDAN getirildi ve önbelleğe alındı.");
        }
        public async Task<Result<ProductResponseDto>> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return Result<ProductResponseDto>.Failure($"{id} numaralı ürün bulunamadı.");
            }
            var responseDto = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CreatedDate = product.CreatedDate
            };
            return Result<ProductResponseDto>.Success(responseDto);
        }
        public async Task<Result<ProductResponseDto>> CreateAsync(ProductCreateDto dto)
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<ProductResponseDto>.Failure("Validasyon hatası oluştu.", errors);
            }
            var newProduct = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock
            };
            await _unitOfWork.Products.AddAsync(newProduct);
            await _unitOfWork.SaveChangesAsync();
            _cache.Remove(ProductsCacheKey);
            var responseDto = new ProductResponseDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                Stock = newProduct.Stock,
                CreatedDate = newProduct.CreatedDate
            };
            return Result<ProductResponseDto>.Success(responseDto, "Ürün başarıyla oluşturuldu.");
        }
        public async Task<Result> UpdateAsync(int id, ProductUpdateDto dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result.Failure("Validasyon hatası oluştu.", errors);
            }
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return Result.Failure("Güncellenecek ürün bulunamadı.");
            }
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();
            _cache.Remove(ProductsCacheKey);
            return Result.Success("Ürün başarıyla güncellendi.");
        }
        public async Task<Result> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return Result.Failure("Silinecek ürün bulunamadı.");
            }
            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            _cache.Remove(ProductsCacheKey);
            return Result.Success("Ürün başarıyla silindi.");
        }
    }
}
