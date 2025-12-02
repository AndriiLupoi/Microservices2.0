using Aggregator.API.Clients;
using Aggregator.API.DTOs;
using Aggregator.API.DTOs.OrderFullDetails;
using Aggregator.API.DTOs.ProductWithReviews;
using Microsoft.AspNetCore.Mvc;

namespace Aggregator.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AggregatorController : ControllerBase
{
    private readonly CatalogClient _catalogClient;
    private readonly OrdersClient _ordersClient;
    private readonly ReviewsClient _reviewsClient;
    private readonly ILogger<AggregatorController> _logger;

    public AggregatorController(
        CatalogClient catalogClient,
        OrdersClient ordersClient,
        ReviewsClient reviewsClient,
        ILogger<AggregatorController> logger)
    {
        _catalogClient = catalogClient;
        _ordersClient = ordersClient;
        _reviewsClient = reviewsClient;
        _logger = logger;
    }

    /// <summary>
    /// Отримати повні деталі замовлення з інформацією про клієнта, продукти та бренди
    /// </summary>
    [HttpGet("order/{orderId}/full")]
    public async Task<ActionResult<OrderFullDetailsDto>> GetOrderFullDetails(
        int orderId,
        CancellationToken cancellationToken)
    {
        var order = await _ordersClient.GetOrderByIdAsync(orderId, cancellationToken);
        if (order == null)
        {
            return NotFound($"Order {orderId} not found");
        }

        var customer = await _ordersClient.GetCustomerByIdAsync(order.CustomerId, cancellationToken);
        var orderItems = await _ordersClient.GetOrderItemsByOrderIdAsync(orderId, cancellationToken);

        var orderDbProductTasks = orderItems
            .Select(item => _ordersClient.GetProductByIdAsync(item.ProductId, cancellationToken))
            .ToList();

        await Task.WhenAll(orderDbProductTasks);

        var orderDbProductsList = orderDbProductTasks
            .Select(t => t.Result)
            .Where(p => p != null)
            .ToList();

        var orderDbProductsDict = orderDbProductsList.ToDictionary(p => p.Id);

        var catalogTasks = orderDbProductsList
            .Select(p => _catalogClient.GetProductByNameAsync(p.Name, cancellationToken))
            .ToList();

        await Task.WhenAll(catalogTasks);

        var catalogProductsList = catalogTasks
            .Select(t => t.Result)
            .Where(p => p != null)
            .ToList();

        var catalogProductsDict = catalogProductsList.ToDictionary(p => p.Name);

        var brandIds = catalogProductsList
            .Select(p => p.BrandId)
            .Distinct()
            .ToList();

        var brandTasks = brandIds
            .Select(id => _catalogClient.GetBrandByIdAsync(id, cancellationToken))
            .ToList();

        await Task.WhenAll(brandTasks);

        var brandsList = brandTasks
            .Select(t => t.Result)
            .Where(b => b != null)
            .ToList();


        var brandsDict = brandsList.ToDictionary(b => b.BrandId);

        var itemsResult = new List<OrderItemDetailsDto>();

        foreach (var orderItem in orderItems)
        {

            // Знаходимо продукт з Orders DB
            if (!orderDbProductsDict.TryGetValue(orderItem.ProductId, out var productFromOrdersDb))
            {
                _logger.LogWarning("❌ ProductId {ProductId} from OrderItems not found in Orders DB Dictionary (Keys: {Keys})",
                    orderItem.ProductId, string.Join(", ", orderDbProductsDict.Keys));
                continue;
            }

            _logger.LogInformation("  ✅ Found in Orders DB: Name='{Name}'", productFromOrdersDb.Name);

            // Знаходимо продукт з Catalog
            if (!catalogProductsDict.TryGetValue(productFromOrdersDb.Name, out var productFromCatalog))
            {
                _logger.LogWarning("❌ Product '{ProductName}' from Orders DB not found in CatalogService",
                    productFromOrdersDb.Name);
                continue;
            }


            // Знаходимо Brand
            if (!brandsDict.TryGetValue(productFromCatalog.BrandId, out var brand))
            {
                _logger.LogWarning("❌ BrandId {BrandId} for Product '{ProductName}' not found",
                    productFromCatalog.BrandId, productFromCatalog.Name);
                continue;
            }


            // Додаємо до результату
            itemsResult.Add(new OrderItemDetailsDto
            {
                ProductId = orderItem.ProductId,
                ProductName = productFromCatalog.Name,
                SKU = productFromCatalog.SKU,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price > 0 ? orderItem.Price : productFromCatalog.Price,
                Brand = new BrandDetailsDto
                {
                    BrandId = brand.BrandId,
                    BrandName = brand.Name
                }
            });

            _logger.LogInformation("  ✅ OrderItemDetailsDto created successfully");
        }

        var result = new OrderFullDetailsDto
        {
            OrderId = order.ordersId,
            OrderDate = order.OrderDate,
            Customer = new CustomerDetailsDto
            {
                CustomerId = customer.CustomerId,
                Name = customer?.Name ?? "Unknown",
                Email = customer?.Email ?? "N/A"
            },
            Items = itemsResult
        };

        return Ok(result);
    }



    /// <summary>
    /// Отримати продукт з усіма відгуками та статистикою
    /// </summary>
    [HttpGet("product/{productId}/with-reviews")]
    public async Task<ActionResult<ProductWithReviewsDto>> GetProductWithReviews(
    int productId,
    CancellationToken cancellationToken)
    {
        _logger.LogInformation("Aggregating product {ProductId} with reviews", productId);
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var product = await _catalogClient.GetProductByIdAsync(productId, cancellationToken);
        if (product == null)
        {
            return NotFound($"Product {productId} not found");
        }

        var mongoProductId = await _reviewsClient.GetProductIdByNameAsync(product.Name, cancellationToken);

        List<ReviewDto> reviews;
        if (mongoProductId != null)
        {
            reviews = await _reviewsClient.GetReviewsByProductIdAsync(mongoProductId, cancellationToken);
        }
        else
        {
            reviews = new List<ReviewDto>();
        }

        var brand = await _catalogClient.GetBrandByIdAsync(product.BrandId, cancellationToken);

        var result = new ProductWithReviewsDto
        {
            ProductId = product.ProductId,
            ProductName = product.Name,
            SKU = product.SKU,
            Price = product.Price,
            Brand = new BrandDetailsDto
            {
                BrandId = brand?.BrandId ?? 0,
                BrandName = brand?.Name ?? "Unknown"
            },
            ReviewStatistics = new ReviewStatisticsDto
            {
                TotalReviews = reviews.Count,
                AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0
            },
            Reviews = reviews.Select(r => new ReviewDetailsDto
            {
                ReviewId = r.Id,
                UserName = r.UserName,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList()
        };

        stopwatch.Stop();
        _logger.LogInformation(
            "Successfully aggregated product {ProductId} with {ReviewCount} reviews in {ElapsedMs}ms",
            productId, reviews.Count, stopwatch.ElapsedMilliseconds);

        return Ok(result);
    }


}