using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
