using Serilog.Context;

namespace ApiGateway.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Перевірка наявності CorrelationId від клієнта
        var correlationId = context.Request.Headers["X-Correlation-Id"].FirstOrDefault();

        if (string.IsNullOrEmpty(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            _logger.LogInformation("Generated new CorrelationId: {CorrelationId}", correlationId);
        }
        else
        {
            _logger.LogInformation("Using client-provided CorrelationId: {CorrelationId}", correlationId);
        }

        // Додати до request headers для передачі downstream
        context.Request.Headers["X-Correlation-Id"] = correlationId;

        // Додати до response headers
        context.Response.Headers["X-Correlation-Id"] = correlationId;

        // Додати до LogContext
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            _logger.LogInformation(
                "Gateway received request: {Method} {Path} from {ClientIP}",
                context.Request.Method,
                context.Request.Path,
                context.Connection.RemoteIpAddress
            );

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "Gateway response: {Method} {Path} -> {StatusCode} in {ElapsedMs}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds
            );
        }
    }
}