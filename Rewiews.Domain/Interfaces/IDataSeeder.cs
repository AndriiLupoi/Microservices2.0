using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Domain.Interfaces
{
    public interface IDataSeeder
    {
        Task SeedAsync(CancellationToken cancellationToken = default);
    }

}
