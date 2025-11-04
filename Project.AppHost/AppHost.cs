using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

var builder = DistributedApplication.CreateBuilder(args);

var ordersApi = builder.AddProject<Projects.Orders_API>("orders-api");
var catalogApi = builder.AddProject<Projects.Catalog_API>("catalog-api");

var ordersDb = builder.AddConnectionString(
    "OrdersDb",
    @"Server=localhost\SQLEXPRESS;Database=OrdersDb;Trusted_Connection=True;TrustServerCertificate=True;"
);

var catalogDb = builder.AddConnectionString(
    "CatalogDb",
    @"Server=localhost\SQLEXPRESS;Database=CatalogDb;Trusted_Connection=True;TrustServerCertificate=True;"
);

//// 🔹 Додаємо Redis як контейнер
//var redis = builder.AddContainer("orders-redis", "redis")
//    .WithImageTag("latest")       // остання версія Redis
//    .WithEndpoint(port: 6379, targetPort: 6379);

//// 🔹 Пов’язуємо API з Redis
//ordersApi.WithReference(redis);

ordersApi.WithReference(ordersDb);
catalogApi.WithReference(catalogDb);


builder.Build().Run();
