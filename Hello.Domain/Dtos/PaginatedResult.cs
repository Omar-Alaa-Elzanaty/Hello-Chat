using FluentValidation.Results;
using Hello.Domain.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hello.Domain.Dtos
{
    internal class PaginatedResult<T>
    {
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public IEnumerable<T>? Data { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }

        public PaginatedResult<T> ToValidationErrors(Dictionary<string, List<string>> errors, HttpStatusCode statusCode,
            string message)
        {
            return new PaginatedResult<T>(message, statusCode, errors);
        }

        public PaginatedResult()
        {
            
        }

        public PaginatedResult(
            IEnumerable<T> items,
            int totalCount,
            int pageNumber,
            int pageSize,
            string? message = null,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Data = items;
            StatusCode = statusCode;
            Message = message;
        }

        public PaginatedResult(
            string message,
            HttpStatusCode statusCode,
            Dictionary<string, List<string>>? errors = null)
        {
            Message = message;
            StatusCode = statusCode;
            Errors = errors;
        }


        public static PaginatedResult<T> Success(
            List<T> data,
            int count,
            int pageNumber,
            int pageSize,
            string? message = null)
        {
            return new PaginatedResult<T>(data, count, pageNumber, pageSize, message);
        }   

        public static Task<PaginatedResult<T>> SuccessAsync(
            List<T> items,
            int totalCount,
            int pageNumber,
            int pageSize,
            string? message = null)
        {
            return Task.FromResult(new PaginatedResult<T>(
                items: items,
                totalCount: totalCount,
                pageNumber: pageNumber,
                pageSize: pageSize,
                message: message));
        }

        public static PaginatedResult<T> Failure(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            Dictionary<string, List<string>>? errors = null)
        {
            return new PaginatedResult<T>(message, statusCode, errors);
        }

        public static Task<PaginatedResult<T>> FailureAsync(
            string message,
            HttpStatusCode statusCode = HttpStatusCode.BadRequest,
            Dictionary<string, List<string>>? errors = null)
        {
            return Task.FromResult(Failure(message, statusCode, errors));
        }

        public static PaginatedResult<T> ValidationFailure(
            List<ValidationFailure> validationFailures,
            string? message = null)
        {
            return new PaginatedResult<T>(
                message ?? "Validation failed",
                HttpStatusCode.UnprocessableEntity,
                validationFailures.GetErrorsDictionary());
        }

        public static PaginatedResult<T> ValidationFailure(IEnumerable<IdentityError> errors)
        {
            return new()
            {
                Errors = errors.ToList().GetErrorsDictionary(),
                StatusCode = HttpStatusCode.UnprocessableEntity
            };
        }

        public static PaginatedResult<T> ValidationFailure(IEnumerable<IdentityError> errors, string message)
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
