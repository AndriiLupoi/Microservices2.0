using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// SQL Server для Catalog
var sqlCatalog = builder.AddSqlServer("sqlCatalog")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("catalog-sql-data")
    .AddDatabase("CatalogDb");

// SQL Server для Orders
var sqlOrders = builder.AddSqlServer("sqlOrders")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("orders-sql-data")
    .WithBindMount("./Orders.API/db_orders", "/docker-entrypoint-initdb.d")
    .AddDatabase("OrdersDb");

// MongoDB для Reviews
var mongo = builder.AddMongoDB("mongo")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume("mongo-data")               
    .AddDatabase("MongoDb");

var ordersApi = builder.AddProject<Orders_API>("orders-api")
    .WithReference(sqlOrders)
    .WaitFor(sqlOrders);                        

var catalogApi = builder.AddProject<Catalog_API>("catalog-api")
    .WithReference(sqlCatalog)
    .WaitFor(sqlCatalog);

var reviewsApi = builder.AddProject<Rewiews_API>("reviews-api")
    .WithReference(mongo)
    .WaitFor(mongo);

var aggregator = builder.AddProject<Aggregator_API>("aggregator")
    .WithReference(ordersApi)
    .WithReference(catalogApi)
    .WithReference(reviewsApi)
    .WaitFor(ordersApi)
    .WaitFor(catalogApi)
    .WaitFor(reviewsApi);

var gateway = builder.AddProject<ApiGateway>("gateway")
    .WithHttpEndpoint(port: 5000, name: "gateway-http")
    .WithReference(ordersApi)
    .WithReference(catalogApi)
    .WithReference(reviewsApi)
    .WithReference(aggregator)
    .WaitFor(aggregator);


builder.Build().Run();