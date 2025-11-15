using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.Common.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException(string message) : base(message)
        {}

        public NotFoundException(string entity, string key)
            : base($"{entity} with identifier '{key}' was not found.") { }
    }
}
