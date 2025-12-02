using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoProducts.Queries.GetTodoProducts
{
    public class GetProductByNameQuery : IQuery<ProductDto>
    {
        public string Name { get; set; } = null!;

        public GetProductByNameQuery(string productName)
        {
            Name = productName;
        }
    }
}
