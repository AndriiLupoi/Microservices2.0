using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Infrastructure.Common.Exceptions
{
    public class MongoDbWriteException : Exception
    {
        public MongoDbWriteException(string message, Exception? inner = null)
            : base(message, inner) { }
    }
}
