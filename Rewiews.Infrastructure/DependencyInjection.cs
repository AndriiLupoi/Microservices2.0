using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rewiews.Application.Common.Mappings;
using Rewiews.Domain.Interfaces;
using Rewiews.Infrastructure.Common.Seeders;
using Rewiews.Infrastructure.Context;
using Rewiews.Infrastructure.Repositories;
using Rewiews.Infrastructure.Services;

namespace Rewiews.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            var databaseName = configuration["DatabaseName"];

            services.AddSingleton(new MongoDbContext(connectionString!, databaseName!));

            MongoDbMappings.RegisterClassMaps();

            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            services.AddSingleton<IIdGenerator, MongoIdGenerator>();

            services.AddSingleton<ProductSeeder>();
            services.AddSingleton<UserProfileSeeder>();
            services.AddSingleton<ReviewSeeder>();

            services.AddSingleton<IDataSeeder>(sp => sp.GetRequiredService<ProductSeeder>());
            services.AddSingleton<IDataSeeder>(sp => sp.GetRequiredService<UserProfileSeeder>());
            services.AddSingleton<IDataSeeder>(sp => sp.GetRequiredService<ReviewSeeder>());

            services.AddSingleton<SeedManager>();



            return services;
        }
    }
}
