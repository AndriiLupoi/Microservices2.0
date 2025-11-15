using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Infrastructure.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Rewiews.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            string title = "An unexpected error occurred.";
            string detail = exception.Message;
            string errorType = exception.GetType().Name;

            _logger.LogError(exception, "❌ Exception caught in middleware: {Message}", exception.Message);

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Validation Failed";
                    detail = validationException.Errors != null && validationException.Errors.Any()
                        ? string.Join("; ", validationException.Errors.SelectMany(kvp => kvp.Value.Select(msg => $"{kvp.Key}: {msg}")))
                        : validationException.Message;
                    break;

                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    title = "Resource Not Found";
                    detail = notFoundException.Message;
                    break;

                case ConflictException conflictException:
                    statusCode = HttpStatusCode.Conflict;
                    title = "Conflict";
                    detail = conflictException.Message;
                    break;

                case DomainException domainException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Domain Error";
                    detail = domainException.Message;
                    break;


                case MongoDbConnectionException mongoConnEx:
                    statusCode = HttpStatusCode.ServiceUnavailable;
                    title = "MongoDB Connection Error";
                    break;

                case MongoDbWriteException mongoWriteEx:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "MongoDB Write Error";
                    break;

                case ArgumentNullException:
                case ArgumentException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Invalid Argument";
                    break;

                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    title = "Unauthorized";
                    break;
            }

            var problem = new
            {
                type = $"https://httpstatuses.com/{(int)statusCode}",
                title,
                status = (int)statusCode,
                detail,
                instance = context.Request.Path,
                errorType
            };

            var json = JsonSerializer.Serialize(problem);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(json);
        }
    }
}
