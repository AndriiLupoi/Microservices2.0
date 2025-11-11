using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Rewiews.Application.TodoProducts.Commands.ProductCommands.CreateTodo;
using Rewiews.Domain.Interfaces;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// 🔹 Реєстрація MediatR
//builder.Services.AddMediatR(typeof(CreateProductCommandHandler).Assembly);

// Інші сервіси (реалізації репозиторіїв в Infrastructure)
//builder.Services.AddScoped<IProductRepository, Infrastructure.Repositories.ProductRepository>();
//builder.Services.AddScoped<IUserProfileRepository, Infrastructure.Repositories.UserProfileRepository>();

var app = builder.Build();


app.Run();
