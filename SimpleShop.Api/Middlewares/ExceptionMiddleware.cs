using SimpleShop.Application.Common;
using System.Net;
using System.Text.Json;

namespace SimpleShop.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // İsteğin normal şekilde Controller'a gitmesine izin ver
                await _next(context);
            }
            catch (Exception ex)
            {
                // Beklenmeyen bir hata olursa yakala ve yönet
                _logger.LogError(ex, "Beklenmeyen bir sistem hatası meydana geldi: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500
            var response = Result.Failure(
                message: "Sunucu tarafında beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.",
                errors: new List<string> { exception.Message }
            );
            // JSON çıktısını küçük harfli (camelCase) yap
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, jsonOptions);
            return context.Response.WriteAsync(json);
        }
    }
}
