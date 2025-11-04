using Catalog.API.Middleware;
using Catalog.Bll.Interfaces;
using Catalog.Bll.Mapping;
using Catalog.Bll.Services;
using Catalog.Dal.Context;
using Catalog.Dal.Repo.UOW;
using Catalog.Domain.Entity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // логуватимуться тільки Information, Warning, Error
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // мінімізує зайві логи від Microsoft/EFCore
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(
        path: "Logs/log.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();

builder.Services.AddControllers();

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Catalog API",
        Version = "v1",
        Description = "API для управління брендами, категоріями та продуктами."
    });

    // Підключаємо XML коментарі
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});


builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDb"))
);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1"));
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

    // Міграції
    db.Database.Migrate();

    // Конфігураційний прапорець (можна вимкнути у Production)
    var seedData = builder.Configuration.GetValue<bool>("SeedData", true);
    if (seedData)
    {
        // --- Бренди ---
        if (!db.Brands.Any())
        {
            db.Brands.AddRange(
                new Brand {Name = "Bosch" },
                new Brand {Name = "Valeo" },
                new Brand {Name = "NGK" }
            );
            db.SaveChanges();
        }

        // --- Категорії ---
        if (!db.Categories.Any())
        {
            db.Categories.AddRange(
                new Category {Name = "Свічки запалювання" },
                new Category {Name = "Фільтри" },
                new Category {Name = "Гальмівні колодки" }
            );
            db.SaveChanges();
        }

        // --- Продукти ---
        if (!db.Products.Any())
        {
            db.Products.AddRange(
                new Product {Name = "Свічка Bosch Super", SKU = "BOSCH-SPARK-001", Price = 150, BrandId = 1 },
                new Product {Name = "Фільтр повітря Valeo", SKU = "VALEO-AIR-001", Price = 200, BrandId = 2 },
                new Product {Name = "Колодки гальмівні NGK", SKU = "NGK-BRAKE-001", Price = 400, BrandId = 3 }
            );
            db.SaveChanges();
        }

        // --- ProductCategory (зв'язки продуктів і категорій) ---
        if (!db.ProductCategories.Any())
        {
            db.ProductCategories.AddRange(
                new ProductCategory { Id = 1, ProductId = 1, CategoryId = 1 }, // Свічки Bosch → Свічки запалювання
                new ProductCategory { Id = 2, ProductId = 2, CategoryId = 2 }, // Фільтр Valeo → Фільтри
                new ProductCategory { Id = 3, ProductId = 3, CategoryId = 3 }  // Колодки NGK → Гальмівні колодки
            );
            db.SaveChanges();
        }
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();