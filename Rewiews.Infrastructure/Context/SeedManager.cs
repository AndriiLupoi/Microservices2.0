using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Infrastructure.Context
{
    public class SeedManager
    {
        private readonly IEnumerable<IDataSeeder> _seeders;

        public SeedManager(IEnumerable<IDataSeeder> seeders)
        {
            _seeders = seeders;
        }

        public async Task SeedAllAsync()
        {
            foreach (var seeder in _seeders)
            {
                await seeder.SeedAsync();
            }
        }
    }

}
