using Orders.Bll.Interfaces;
using Orders.Bll.Mappers;
using Orders.Bll.Services;
using Orders.Dal.Repo.Interfaces;
using Orders.Dal.UOW;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

// Додати ServiceDefaults на початку
builder.AddServiceDefaults();

// Репозиторії всередині UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Сервіси BLL
builder.Services.AddScoped<ICustomersService, CustomersService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IOrderItemsService, OrderItemsService>();
builder.Services.AddScoped<IProductsService, ProductsService>();

builder.Services.AddAutoMapper(typeof(OrdersMappingProfile));

// Controllers та Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});


var app = builder.Build();

// Додати UseServiceDefaults після Build()
app.UseServiceDefaults();

// Swagger тільки для Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();