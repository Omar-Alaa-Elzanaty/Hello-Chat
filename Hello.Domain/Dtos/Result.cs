using FluentValidation.Results;
using Hello.Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Hello.Domain.Dtos
{
    public class Result<T>
    {
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }

        public Result()
        {
        }

        public static Result<T> Success()
        {
            return new()
            {
                StatusCode = HttpStatusCode.OK
            };
        }

        public static Result<T> Success(T data, string? message = null)
        {
            return new()
            {
                Data = data,
                Message = message,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static Result<T> Success(string message)
        {
            return new()
            {
                StatusCode = HttpStatusCode.OK,
                Message = message
            };
        }

        public static Result<T> Success(T data, string message, HttpStatusCode statusCode)
        {
            return new()
            {
                Data = data,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static Result<T> Failure(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new()
            {
                Message = message,
                StatusCode = statusCode
            };
        }

        public static Result<T> ValidationFailure(List<ValidationFailure> errors)
        {
            return new()
            {
                Errors = errors.GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity
            };
        }

        public static Result<T> ValidationFailure(List<ValidationFailure> errors, string message)
        {
            return new()
            {
                Errors = errors.GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = message
            };
        }

        public static Result<T> ValidationFailure(IEnumerable<IdentityError> errors)
        {
            return new()
            {
                Errors = errors.ToList().GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity
            };
        }

        public static Result<T> ValidationFailure(IEnumerable<IdentityError> errors, string message)
        {
            return new()
            {
                Errors = errors.ToList().GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = message
            };
        }
    }
}
