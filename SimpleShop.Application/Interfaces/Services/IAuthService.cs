using SimpleShop.Application.Common;
using SimpleShop.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<string>> RegisterAsync(RegisterDto dto);
        Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
    }
}
