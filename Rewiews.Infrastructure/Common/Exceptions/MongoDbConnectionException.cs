using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Infrastructure.Common.Exceptions
{
    public class MongoDbConnectionException : Exception
    {
        public MongoDbConnectionException(string message, Exception? inner = null)
            : base(message, inner) { }
    }
}
