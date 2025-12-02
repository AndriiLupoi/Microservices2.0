namespace Aggregator.API.Clients;

public class ReviewsClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReviewsClient> _logger;

    public ReviewsClient(HttpClient httpClient, ILogger<ReviewsClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Отримує Mongo ID продукту за його назвою
    /// </summary>
    public async Task<string?> GetProductIdByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        _logger.LogInformation("Getting Mongo product ID for product name '{ProductName}'", name);

        try
        {
            var encodedName = Uri.EscapeDataString(name);
            var url = $"/api/reviews/product/name/{encodedName}";

            _logger.LogInformation("Calling Reviews API: {Url}", url);

            var response = await _httpClient.GetAsync(url, cancellationToken);

            stopwatch.Stop();

            // Логуємо повну інформацію про відповідь
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation(
                "Reviews API response: StatusCode={StatusCode}, Body='{Body}'",
                response.StatusCode, responseBody);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Reviews service returned {StatusCode} for product name '{ProductName}' in {ElapsedMs}ms",
                    response.StatusCode, name, stopwatch.ElapsedMilliseconds);
                return null;
            }

            // Спробуйте десеріалізувати як об'єкт
            var result = await response.Content.ReadFromJsonAsync<ProductIdResponse>(cancellationToken);
            var productId = result?.Id;

            _logger.LogInformation(
                "Found Mongo product ID '{ProductId}' for product name '{ProductName}' in {ElapsedMs}ms",
                productId, name, stopwatch.ElapsedMilliseconds);

            return productId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error getting Mongo product ID for product name '{ProductName}' after {ElapsedMs}ms",
                name, stopwatch.Elapsed);
            return null;
        }
    }


    /// <summary>
    /// Отримує список відгуків за Mongo ID продукту
    /// </summary>
    public async Task<List<ReviewDto>> GetReviewsByProductIdAsync(string productId, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        _logger.LogInformation("Getting reviews for product ID {ProductId}", productId);

        try
        {
            var response = await _httpClient.GetAsync($"/api/reviews?productId={productId}", cancellationToken);
            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Reviews service returned {StatusCode} for product ID {ProductId} in {ElapsedMs}ms",
                    response.StatusCode, productId, stopwatch.ElapsedMilliseconds);
                return new List<ReviewDto>();
            }

            var reviews = await response.Content.ReadFromJsonAsync<List<ReviewDto>>(cancellationToken)
                          ?? new List<ReviewDto>();

            _logger.LogInformation(
                "Reviews service responded with {ReviewCount} reviews for product ID {ProductId} in {ElapsedMs}ms",
                reviews.Count, productId, stopwatch.ElapsedMilliseconds);

            return reviews;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error getting reviews for product ID {ProductId} after {ElapsedMs}ms",
                productId, stopwatch.ElapsedMilliseconds);
            return new List<ReviewDto>();
        }
    }
}

public record ReviewDto(string Id, string ProductId, string UserName, int Rating, string Comment, DateTime CreatedAt);

public class ProductIdResponse
{    public string Id { get; set; } = string.Empty;} 