using System;

namespace Orders.Bll.Exceptions
{
    public class BusinessConflictException : Exception
    {
        public BusinessConflictException(string message)
            : base(message)
        {
        }
    }
}
