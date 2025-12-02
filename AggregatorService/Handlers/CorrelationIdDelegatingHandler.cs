namespace Aggregator.API.Handlers;

public class CorrelationIdDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CorrelationIdDelegatingHandler> _logger;

    public CorrelationIdDelegatingHandler(
        IHttpContextAccessor httpContextAccessor,
        ILogger<CorrelationIdDelegatingHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Отримати CorrelationId з поточного HTTP контексту
        var correlationId = _httpContextAccessor.HttpContext?.Request.Headers["X-Correlation-Id"].FirstOrDefault();

        if (!string.IsNullOrEmpty(correlationId))
        {
            // Додати до downstream запиту
            request.Headers.Add("X-Correlation-Id", correlationId);
            _logger.LogDebug("Propagating CorrelationId {CorrelationId} to {RequestUri}",
                correlationId, request.RequestUri);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}