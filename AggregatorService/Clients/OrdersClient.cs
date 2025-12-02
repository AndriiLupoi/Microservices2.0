
using Aggregator.API.DTOs.OrderFullDetails;

namespace Aggregator.API.Clients;

public class OrdersClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrdersClient> _logger;

    public OrdersClient(HttpClient httpClient, ILogger<OrdersClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        _logger.LogInformation("Calling Orders service for order {OrderId}", orderId);

        try
        {
            var response = await _httpClient.GetAsync($"/api/orders/{orderId}", cancellationToken);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Orders service returned {StatusCode} for order {OrderId} in {ElapsedMs}ms",
                    response.StatusCode, orderId, stopwatch.ElapsedMilliseconds);
                return null;
            }

            var order = await response.Content.ReadFromJsonAsync<OrderDto>(
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                },
                cancellationToken
            );


            _logger.LogInformation(
                "Orders service responded successfully for order {OrderId} in {ElapsedMs}ms",
                orderId, stopwatch.ElapsedMilliseconds);

            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error calling Orders service for order {OrderId} after {ElapsedMs}ms",
                orderId, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        _logger.LogInformation("Calling Orders service for customer {CustomerId}", customerId);

        try
        {
            var response = await _httpClient.GetAsync($"/api/customers/{customerId}", cancellationToken);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Orders service returned {StatusCode} for customer {CustomerId} in {ElapsedMs}ms",
                    response.StatusCode, customerId, stopwatch.ElapsedMilliseconds);
                return null;
            }

            var customer = await response.Content.ReadFromJsonAsync<CustomerDto>(cancellationToken);

            _logger.LogInformation(
                "Orders service responded successfully for customer {CustomerId} in {ElapsedMs}ms",
                customerId, stopwatch.ElapsedMilliseconds);

            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error calling Orders service for customer {CustomerId} after {ElapsedMs}ms",
                customerId, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public async Task<List<OrderItemDto>> GetOrderItemsByOrderIdAsync(
    int orderId,
    CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        _logger.LogInformation("Calling Orders service for order items of OrderId={OrderId}", orderId);

        try
        {
            var response = await _httpClient.GetAsync($"/api/order-items/order/{orderId}", cancellationToken);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Orders service returned {StatusCode} for order items of OrderId={OrderId} in {ElapsedMs}ms",
                    response.StatusCode, orderId, stopwatch.ElapsedMilliseconds);

                return new List<OrderItemDto>(); // повертаємо пустий список, але не null
            }

            var items = await response.Content.ReadFromJsonAsync<List<OrderItemDto>>(cancellationToken);

            _logger.LogInformation(
                "Orders service responded successfully with {Count} order items for OrderId={OrderId} in {ElapsedMs}ms",
                items?.Count ?? 0, orderId, stopwatch.ElapsedMilliseconds);

            return items ?? new List<OrderItemDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error calling Orders service for order items of OrderId={OrderId} after {ElapsedMs}ms",
                orderId, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }

    public async Task<ProductFromOrderDbDto?> GetProductByIdAsync(
    int productId,
    CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        _logger.LogInformation("Calling Orders service for product {ProductId}", productId);

        try
        {
            var url = $"/api/orders/products/{productId}";
            _logger.LogInformation("Request URL: {Url}", url);

            var response = await _httpClient.GetAsync(url, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation("Response: StatusCode={StatusCode}, Body={Body}",
                response.StatusCode, responseBody);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Orders service returned {StatusCode} for product {ProductId} in {ElapsedMs}ms",
                    response.StatusCode, productId, stopwatch.ElapsedMilliseconds);
                return null;
            }

            var product = await response.Content.ReadFromJsonAsync<ProductFromOrderDbDto>(cancellationToken);

            if (product != null)
            {
                _logger.LogInformation(
                    "✅ Orders service returned product: ProductId={ProductId}, Name={Name}, Price={Price}",
                    product.Id, product.Name, product.Price);
            }
            else
            {
                _logger.LogWarning("Orders service returned NULL for product {ProductId}", productId);
            }

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error calling Orders service for product {ProductId} after {ElapsedMs}ms",
                productId, stopwatch.ElapsedMilliseconds);
            return null; // ⚠️ Поверніть null замість throw
        }
    }


}

// DTOs для Orders
public record OrderDto(int ordersId, int CustomerId, DateTime OrderDate, List<OrderItemDto> Items);
public record OrderItemDto(int ProductId, int Quantity, decimal Price);
public record CustomerDto(int CustomerId, string Name, string Email);