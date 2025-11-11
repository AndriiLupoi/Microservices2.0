using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rewiews.Domain.Interfaces;
using Rewiews.Infrastructure.Context;
using Rewiews.Infrastructure.Repositories;

namespace Rewiews.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            var databaseName = configuration["DatabaseName"];

            services.AddSingleton(new MongoDbContext(connectionString!, databaseName!));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            MongoDbMappings.RegisterClassMaps();


            return services;
        }
    }
}
