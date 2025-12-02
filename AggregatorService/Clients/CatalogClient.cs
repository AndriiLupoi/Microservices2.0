using System.Text.Json;

namespace Aggregator.API.Clients;

public class CatalogClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatalogClient> _logger;

    public CatalogClient(HttpClient httpClient, ILogger<CatalogClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        _logger.LogInformation("Calling Catalog service for product {ProductId}", productId);

        try
        {
            var response = await _httpClient.GetAsync($"/api/products/{productId}", cancellationToken);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Catalog service returned {StatusCode} for product {ProductId} in {ElapsedMs}ms",
                    response.StatusCode, productId, stopwatch.ElapsedMilliseconds);
                return null;
            }

            var product = await response.Content.ReadFromJsonAsync<ProductDto>(cancellationToken);

            _logger.LogInformation(
                "Catalog service responded successfully for product {ProductId} in {ElapsedMs}ms",
                productId, stopwatch.ElapsedMilliseconds);

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error calling Catalog service for product {ProductId} after {ElapsedMs}ms",
                productId, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public async Task<BrandDto?> GetBrandByIdAsync(int brandId, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        _logger.LogInformation("Calling Catalog service for brand {BrandId}", brandId);

        try
        {
            var response = await _httpClient.GetAsync($"/api/brands/{brandId}", cancellationToken);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Catalog service returned {StatusCode} for brand {BrandId} in {ElapsedMs}ms",
                    response.StatusCode, brandId, stopwatch.ElapsedMilliseconds);
                return null;
            }

            var brand = await response.Content.ReadFromJsonAsync<BrandDto>(cancellationToken);

            _logger.LogInformation(
                "Catalog service responded successfully for brand {BrandId} in {ElapsedMs}ms",
                brandId, stopwatch.ElapsedMilliseconds);

            return brand;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error calling Catalog service for brand {BrandId} after {ElapsedMs}ms",
                brandId, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public async Task<ProductDto?> GetProductByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        _logger.LogInformation("Calling Catalog service for product with name '{ProductName}'", name);

        try
        {
            var encodedName = Uri.EscapeDataString(name);
            var url = $"/api/products/name/{encodedName}";

            var response = await _httpClient.GetAsync(url, cancellationToken);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Catalog service returned {StatusCode} for product with name '{ProductName}' in {ElapsedMs}ms",
                    response.StatusCode, name, stopwatch.ElapsedMilliseconds);
                return null;
            }

            // 🔧 ВИПРАВЛЕННЯ: читаємо як один ProductDto, а не список
            var product = await response.Content.ReadFromJsonAsync<ProductDto>(cancellationToken);

            if (product != null)
            {
                _logger.LogInformation(
                    "✅ Catalog service responded successfully: ProductId={ProductId}, Name='{ProductName}', BrandId={BrandId} in {ElapsedMs}ms",
                    product.ProductId, product.Name, product.BrandId, stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogWarning(
                    "Catalog service returned NULL for product with name '{ProductName}' in {ElapsedMs}ms",
                    name, stopwatch.ElapsedMilliseconds);
            }

            return product;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error calling Catalog service for product with name '{ProductName}' after {ElapsedMs}ms",
                name, stopwatch.ElapsedMilliseconds);
            return null;
        }
    }
}

// DTOs для Catalog
public record ProductDto(int ProductId, string Name, string SKU, decimal Price, int BrandId);
public record BrandDto(int BrandId, string Name);