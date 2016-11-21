using System;

namespace Scrumfish.OData.Client.Common
{
    internal class ArgumentCountException : Exception
    {
        public ArgumentCountException(string message) : base(message) {}
    }
}