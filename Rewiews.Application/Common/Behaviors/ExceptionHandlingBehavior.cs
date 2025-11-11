using MediatR;
using Microsoft.Extensions.Logging;
using Reviews.Application.Common.Exceptions;

namespace Reviews.Application.Common.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (ValidationException ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogWarning("⚠️ Validation failed for {RequestName}. Errors: {@Errors}", requestName, ex.Errors);
            throw;
        }
        catch (Exception ex)
        {
            // 🔹 Усі інші помилки
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex, "❌ Unhandled exception for request {RequestName}", requestName);
            throw;
        }
    }
}
