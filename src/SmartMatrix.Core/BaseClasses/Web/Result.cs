using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartMatrix.Core.BaseClasses.web
{
    public class Result : IResult
    {
        public Result()
        {
        }
        
        public List<string> Messages { get; set; } = new List<string>();
        public bool Succeeded { get; set; }
        public int? StatusCode { get; set; }
        public bool Failed => !Succeeded;

        public static IResult Fail()
        {
            return new Result { Succeeded = false };
        }

        public static IResult Fail(int statusCode)
        {
            return new Result { Succeeded = false, StatusCode = statusCode };
        }

        public static IResult Fail(string message)
        {
            return new Result { Succeeded = false, Messages = new List<string> { message } };
        }

        public static IResult Fail(int statusCode, string message)
        {
            return new Result { Succeeded = false, StatusCode = statusCode, Messages = new List<string> { message } };
        }

        public static IResult Fail(List<string> messages)
        {
            return new Result { Succeeded = false, Messages = messages };
        }

        public static IResult Fail(int statusCode, List<string> messages)
        {
            return new Result { Succeeded = false, StatusCode = statusCode, Messages = messages };
        }

        public static Task<IResult> FailAsync()
        {
            return Task.FromResult(Fail());
        }
        public static Task<IResult> FailAsync(int statusCode)
        {
            return Task.FromResult(Fail(statusCode));
        }

        public static Task<IResult> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static Task<IResult> FailAsync(int statusCode, string message)
        {
            return Task.FromResult(Fail(statusCode, message));
        }

        public static Task<IResult> FailAsync(List<string> messages)
        {
            return Task.FromResult(Fail(messages));
        }

        public static Task<IResult> FailAsync(int statusCode, List<string> messages)
        {
            return Task.FromResult(Fail(statusCode, messages));
        }

        public static IResult Success()
        {
            return new Result { Succeeded = true };
        }

        public static IResult Success(int statusCode)
        {
            return new Result { Succeeded = true, StatusCode = statusCode };
        }

        public static IResult Success(string message)
        {
            return new Result { Succeeded = true, Messages = new List<string> { message } };
        }

        public static IResult Success(int statusCode, string message)
        {
            return new Result { Succeeded = true, StatusCode = statusCode, Messages = new List<string> { message } };
        }

        public static IResult Success(List<string> messages)
        {
            return new Result { Succeeded = true, Messages = messages };
        }

        public static IResult Success(int statusCode, List<string> messages)
        {
            return new Result { Succeeded = true, StatusCode = statusCode, Messages = messages };
        }

        public static Task<IResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResult> SuccessAsync(int statusCode)
        {
            return Task.FromResult(Success(statusCode));
        }

        public static Task<IResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<IResult> SuccessAsync(int statusCode, string message)
        {
            return Task.FromResult(Success(statusCode, message));
        }

        public static Task<IResult> SuccessAsync(List<string> messages)
        {
            return Task.FromResult(Success(messages));
        }

        public static Task<IResult> SuccessAsync(int statusCode, List<string> messages)
        {
            return Task.FromResult(Success(statusCode, messages));
        }
    }

    public class Result<T> : Result, IResult<T>
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