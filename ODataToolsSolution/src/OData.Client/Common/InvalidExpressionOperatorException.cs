using System;

namespace Scrumfish.OData.Client.Common
{
    public class InvalidExpressionOperatorException : Exception
    {
        public InvalidExpressionOperatorException(string message) : base(message)
        {
        }
    }
}