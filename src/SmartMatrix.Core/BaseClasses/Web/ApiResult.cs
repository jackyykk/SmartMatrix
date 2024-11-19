using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMatrix.Core.BaseClasses.Web
{
    public class ApiResult : IApiResult
    {
        public ApiResult()
        {
        }
        
        public List<string> Messages { get; set; } = new List<string>();
        public bool Succeeded { get; set; }
        public int? StatusCode { get; set; } = 0;

        public static IApiResult Fail()
        {
            return new ApiResult { Succeeded = false };
        }

        public static IApiResult Fail(int statusCode)
        {
            return new ApiResult { Succeeded = false, StatusCode = statusCode };
        }

        public static IApiResult Fail(string message)
        {
            return new ApiResult { Succeeded = false, Messages = new List<string> { message } };
        }

        public static IApiResult Fail(int statusCode, string message)
        {
            return new ApiResult { Succeeded = false, StatusCode = statusCode, Messages = new List<string> { message } };
        }

        public static IApiResult Fail(List<string> messages)
        {
            return new ApiResult { Succeeded = false, Messages = messages };
        }

        public static IApiResult Fail(int statusCode, List<string> messages)
        {
            return new ApiResult { Succeeded = false, StatusCode = statusCode, Messages = messages };
        }

        public static Task<IApiResult> FailAsync()
        {
            return Task.FromResult(Fail());
        }
        public static Task<IApiResult> FailAsync(int statusCode)
        {
            return Task.FromResult(Fail(statusCode));
        }

        public static Task<IApiResult> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static Task<IApiResult> FailAsync(int statusCode, string message)
        {
            return Task.FromResult(Fail(statusCode, message));
        }

        public static Task<IApiResult> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        public static Task<IApiResult> FailAsync(int statusCode, List<string> messages)
        {
            return Task.FromResult(Fail(statusCode, messages));
        }

        public static IApiResult Success()
        {
            return new ApiResult { Succeeded = true };
        }

        public static IApiResult Success(int statusCode)
        {
            return new ApiResult { Succeeded = true, StatusCode = statusCode };
        }

        public static IApiResult Success(string message)
        {
            return new ApiResult { Succeeded = true, Messages = new List<string> { message } };
        }

        public static IApiResult Success(int statusCode, string message)
        {
            return new ApiResult { Succeeded = true, StatusCode = statusCode, Messages = new List<string> { message } };
        }

        public static IApiResult Success(List<string> messages)
        {
            return new ApiResult { Succeeded = true, Messages = messages };
        }

        public static IApiResult Success(int statusCode, List<string> messages)
        {
            return new ApiResult { Succeeded = true, StatusCode = statusCode, Messages = messages };
        }

        public static Task<IApiResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IApiResult> SuccessAsync(int statusCode)
        {
            return Task.FromResult(Success(statusCode));
        }

        public static Task<IApiResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<IApiResult> SuccessAsync(int statusCode, string message)
        {
            return Task.FromResult(Success(statusCode, message));
        }

        public static Task<IApiResult> SuccessAsync(List<string> messages)
        {
            return Task.FromResult(Success(messages));
        }

        public static Task<IApiResult> SuccessAsync(int statusCode, List<string> messages)
        {
            return Task.FromResult(Success(statusCode, messages));
        }
    }

    public class Result<T> : ApiResult, IApiResult<T>
    {
        public Result()
        {
            Data = default!;
        }

        public T Data { get; set; }

        public static new Result<T> Fail()
        {
            return new Result<T> { Succeeded = false };
        }

        public static new Result<T> Fail(int statusCode)
        {
            return new Result<T> { Succeeded = false, StatusCode = statusCode };
        }

        public static new Result<T> Fail(string message)
        {
            return new Result<T> { Succeeded = false, Messages = new List<string> { message } };
        }

        public static new Result<T> Fail(int statusCode, string message)
        {
            return new Result<T> { Succeeded = false, StatusCode = statusCode, Messages = new List<string> { message } };
        }

        public static new Result<T> Fail(List<string> messages)
        {
            return new Result<T> { Succeeded = false, Messages = messages };
        }

        public static new Result<T> Fail(int statusCode, List<string> messages)
        {
            return new Result<T> { Succeeded = false, StatusCode = statusCode, Messages = messages };
        }

        public static new Task<Result<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static new Task<Result<T>> FailAsync(int statusCode)
        {
            return Task.FromResult(Fail(statusCode));
        }

        public static new Task<Result<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static new Task<Result<T>> FailAsync(int statusCode, string message)
        {
            return Task.FromResult(Fail(statusCode, message));
        }

        public static new Task<Result<T>> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        public static new Task<Result<T>> FailAsync(int statusCode, List<string> messages)
        {
            return Task.FromResult(Fail(statusCode, messages));
        }

        public static new Result<T> Success()
        {
            return new Result<T> { Succeeded = true };
        }

        public static new Result<T> Success(int statusCode)
        {
            return new Result<T> { Succeeded = true, StatusCode = statusCode };
        }

        public static new Result<T> Success(string message)
        {
            return new Result<T> { Succeeded = true, Messages = new List<string> { message } };
        }

        public static new Result<T> Success(int statusCode, string message)
        {
            return new Result<T> { Succeeded = true, StatusCode = statusCode, Messages = new List<string> { message } };
        }

        public static new Result<T> Success(List<string> messages)
        {
            return new Result<T> { Succeeded = true, Messages = messages };
        }

        public static new Result<T> Success(int statusCode, List<string> messages)
        {
            return new Result<T> { Succeeded = true, StatusCode = statusCode, Messages = messages };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { Succeeded = true, Data = data };
        }

        public static Result<T> Success(T data, int statusCode)
        {
            return new Result<T> { Succeeded = true, Data = data, StatusCode = statusCode };
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T> { Succeeded = true, Data = data, Messages = new List<string> { message } };
        }

        public static Result<T> Success(T data, int statusCode, string message)
        {
            return new Result<T> { Succeeded = true, Data = data, StatusCode = statusCode, Messages = new List<string> { message } };
        }

        public static Result<T> Success(T data, List<string> messages)
        {
            return new Result<T> { Succeeded = true, Data = data, Messages = messages };
        }

        public static Result<T> Success(T data, int statusCode, List<string> messages)
        {
            return new Result<T> { Succeeded = true, Data = data, StatusCode = statusCode, Messages = messages };
        }

        public static new Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static new Task<Result<T>> SuccessAsync(int statusCode)
        {
            return Task.FromResult(Success(statusCode));
        }

        public static new Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static new Task<Result<T>> SuccessAsync(int statusCode, string message)
        {
            return Task.FromResult(Success(statusCode, message));
        }

        public static new Task<Result<T>> SuccessAsync(List<string> messages)
        {
            return Task.FromResult(Success(messages));
        }

        public static new Task<Result<T>> SuccessAsync(int statusCode, List<string> messages)
        {
            return Task.FromResult(Success(statusCode, messages));
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, int statusCode)
        {
            return Task.FromResult(Success(data, statusCode));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }

        public static Task<Result<T>> SuccessAsync(T data, int statusCode, string message)
        {
            return Task.FromResult(Success(data, statusCode, message));
        }

        public static Task<Result<T>> SuccessAsync(T data, List<string> messages)
        {
            return Task.FromResult(Success(data, messages));
        }

        public static Task<Result<T>> SuccessAsync(T data, int statusCode, List<string> messages)
        {
            return Task.FromResult(Success(data, statusCode, messages));
        }
    }
}