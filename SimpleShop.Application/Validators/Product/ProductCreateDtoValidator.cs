using FluentValidation;
using SimpleShop.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Application.Validators.Product
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Ürün adı boş olamaz.")
                .MinimumLength(2).WithMessage("Ürün adı en az 2 karakter olmalıdır.")
                .MaximumLength(150).WithMessage("Ürün adı en fazla 150 karakter olabilir.");
            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");
            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stok miktarı negatif olamaz.");
            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir.");
        }
    }
}
