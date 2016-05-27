using System;

namespace Scrumfish.OData.Client.Common
{
    public class InvalidOperationException : Exception
    {
        public InvalidOperationException(string message) : base(message)
        {
        }
    }
}
