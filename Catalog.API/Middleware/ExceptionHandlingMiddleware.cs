using Catalog.Bll.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;
using System.Net;
using System.Text.Json;

namespace Catalog.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.TraceIdentifier;
            var path = context.Request.Path;
            var method = context.Request.Method;

            try
            {
                await _next(context);

                var status = context.Response.StatusCode;
                if (status >= 200 && status < 300)
                {
                    Log.Information("{Method} {Path} - {StatusCode} {ReasonPhrase}",
                        context.Request.Method,
                        context.Request.Path,
                        status,
                        ReasonPhrases.GetReasonPhrase(status));
                }
                else if (status >= 400 && status < 500)
                {
                    Log.Warning("{Method} {Path} - {StatusCode} {ReasonPhrase}",
                        context.Request.Method,
                        context.Request.Path,
                        status,
                        ReasonPhrases.GetReasonPhrase(status));
                }
            }
            catch (Exception ex)
            {
                // Визначаємо статус для помилки
                var (statusCode, title, detail) = ex switch
                {
                    EntityNotFoundException => ((int)HttpStatusCode.NotFound, "Entity not found", ex.Message),
                    ValidationCustomException => ((int)HttpStatusCode.BadRequest, "Validation error", ex.Message),
                    ConflictException => ((int)HttpStatusCode.Conflict, "Conflict error", ex.Message),
                    _ => ((int)HttpStatusCode.InternalServerError, "Internal server error", _env.IsDevelopment() ? ex.Message : "Unexpected error")
                };

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = statusCode;

                var problem = new
                {
                    type = $"https://httpstatuses.com/{statusCode}",
                    title = ReasonPhrases.GetReasonPhrase(statusCode),
                    status = statusCode,
                    detail = ex is EntityNotFoundException || ex is ValidationCustomException || ex is ConflictException
                        ? ex.Message
                        : (_env.IsDevelopment() ? ex.Message : "Unexpected error")
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(problem, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));

                // Лог без стек-трейсу, тільки коротка інформація
                if (statusCode >= 500)
                    Log.Error("{Method} {Path} - {StatusCode} {ReasonPhrase}",
                        context.Request.Method,
                        context.Request.Path,
                        statusCode,
                        ReasonPhrases.GetReasonPhrase(statusCode));
                else
                    Log.Warning("{Method} {Path} - {StatusCode} {ReasonPhrase}",
                        context.Request.Method,
                        context.Request.Path,
                        statusCode,
                        ReasonPhrases.GetReasonPhrase(statusCode));
            }
        }
    }

    public static class ReasonPhrases
    {
        public static string GetReasonPhrase(int statusCode) => statusCode switch
        {
            200 => "OK",
            201 => "Created",
            204 => "No Content",
            400 => "Bad Request",
            404 => "Not Found",
            409 => "Conflict",
            500 => "Internal Server Error",
            _ => "Unknown"
        };
    }
}
