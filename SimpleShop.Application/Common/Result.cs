using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public static Result Success(string message = "İşlem başarılı.") =>
            new() { IsSuccess = true, Message = message };
        public static Result Failure(string message, List<string>? errors = null) =>
            new() { IsSuccess = false, Message = message, Errors = errors ?? new() };
    }

    public class Result<T> : Result
    {
        public T? Data { get; set; }
        public static Result<T> Success(T data, string message = "İşlem başarılı.") =>
            new() { IsSuccess = true, Message = message, Data = data };
        public new static Result<T> Failure(string message, List<string>? errors = null) =>
            new() { IsSuccess = false, Message = message, Errors = errors ?? new() };
    }
}
