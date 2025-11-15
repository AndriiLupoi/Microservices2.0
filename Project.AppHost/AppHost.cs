using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

var ordersApi = builder.AddProject<Projects.Orders_API>("orders-api");
var catalogApi = builder.AddProject<Projects.Catalog_API>("catalog-api");
var reviewsApi = builder.AddProject<Projects.Rewiews_API>("rewiews-api");

var ordersDb = builder.AddConnectionString(
    "OrdersDb",
    @"Server=localhost\SQLEXPRESS;Database=OrdersDb;Trusted_Connection=True;TrustServerCertificate=True;"
);

var catalogDb = builder.AddConnectionString(
    "CatalogDb",
    @"Server=localhost\SQLEXPRESS;Database=CatalogDb;Trusted_Connection=True;TrustServerCertificate=True;"
);

var reviewsDb = builder.AddConnectionString(
    "ReviewsDb",
    @"mongodb://localhost:27017/ReviewsDb"
);

//// Redis як контейнер
//var redis = builder.AddContainer("orders-redis", "redis")
//    .WithImageTag("latest")       // остання версія Redis
//    .WithEndpoint(port: 6379, targetPort: 6379);

////API з Redis
//ordersApi.WithReference(redis);

ordersApi.WithReference(ordersDb);
catalogApi.WithReference(catalogDb);
reviewsApi.WithReference(reviewsDb);


builder.Build().Run();
