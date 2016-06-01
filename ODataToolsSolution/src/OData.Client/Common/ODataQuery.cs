using System;
using System.Collections.Generic;
using System.Text;

namespace Scrumfish.OData.Client.Common
{
    public class ODataQuery<T>
    {
        private readonly StringBuilder _uri;
        private bool _isStart;
        private HashSet<string> _operations = new HashSet<string>();
        private string _currentOperation = string.Empty;

        internal ODataQuery(string uri)
        {
            _uri = new StringBuilder(uri);
            _isStart = uri.EndsWith("?");
        }

        internal ODataQuery(Uri uri)
        {
            var uriString = uri.ToString();
            _uri = new StringBuilder(uriString);
            _isStart = uriString.EndsWith("?");
        }

        public override string ToString()
        {
            return _uri.ToString();
        }

        public Uri ToUri()
        {
            return new Uri(_uri.ToString());
        }

        internal ODataQuery<T> AppendOperation(string operation)
        {
            if (_operations.Contains(operation))
            {
                throw new InvalidOperationException($"{operation} has alread been added to query.");   
            }
            if (!operation.StartsWith("$"))
            {
                throw new InvalidOperationException($"{operation} is not a valid OData operation.");
            }
            _operations.Add(operation);
            _currentOperation = operation;
            if (!_isStart)
            {
                _uri.Append('&');
            }
            _isStart = false;
            _uri.Append(operation);
            return this;
        }

        internal ODataQuery<T> AssertCurrentOperation(string operation)
        {
            if (_currentOperation != operation)
            {
                throw new InvalidExpressionException($"Expected {operation} but was in {_currentOperation}.");
            }
            return this;
        } 

        internal ODataQuery<T> AppendExpression<TV>(TV expression)
        {
            _uri.Append('=')
                .Append(expression);
            return this;
        }

        internal ODataQuery<T> AppendChainingExpression<TV>(TV expression)
        {
            _uri.Append(',')
                .Append(expression);
            return this;
        }

        internal ODataQuery<T> AppendDesc()
        {
            _uri.Append(" desc");
            return this;
        }
    }

    public static class ODataQueryBuilder
    {
        public static ODataQuery<T> CreateODataQuery<T>(this string baseUri)
        {
            return new ODataQuery<T>(baseUri);
        }

        public static ODataQuery<T> CreateODataQuery<T>(this Uri baseUri)
        {
            return new ODataQuery<T>(baseUri);
        }
    }
}
