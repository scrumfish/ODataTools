using System;

namespace Scrumfish.OData.Client.Common
{
    public class InvalidExpressionException : Exception
    {
        public InvalidExpressionException(string message) : base(message)
        {
        }
    }
}
