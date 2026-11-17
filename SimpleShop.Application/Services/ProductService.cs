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
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IReadOnlyList<ProductResponseDto>>> GetAllAsync()
        {
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
            return Result<IReadOnlyList<ProductResponseDto>>.Success(responseDtos, "Ürünler başarıyla getirildi.");
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
            if (dto.Price <= 0)
            {
                return Result<ProductResponseDto>.Failure("Ürün fiyatı 0 veya negatif olamaz.");
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
            return Result.Success("Ürün başarıyla güncellendi.");
        }
        public async Task<Result> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return Result.Failure("Silinecek ürün bulunamadı.");
            }
            // Soft delete metodumuzu çağırır
            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Ürün başarıyla silindi.");
        }
    }
}
