using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scrumfish.OData.Client.Common
{
    public class ODataQuery<T>
    {
        private readonly StringBuilder _uri;
        private readonly Stack<State> _state = new Stack<State>();
        private State _currentState = new State();

        private class State
        {
            public State()
            {
                Operations = new HashSet<string>();
            }
            public HashSet<string> Operations { get; set; }
            public string CurrentOperation { get; set; }
            public bool InSubQuery { get; set; }
            public bool InQuery { get; set; }
            public bool IsStart { get; set; }
        }

        internal ODataQuery(string uri)
        {
            _uri = new StringBuilder(uri, 512);
            _currentState.IsStart = uri.EndsWith("?");
            _currentState.InQuery = _currentState.IsStart;
        }

        internal ODataQuery(Uri uri)
        {
            var uriString = uri.ToString();
            _uri = new StringBuilder(uriString, 512);
            _currentState.IsStart = uriString.EndsWith("?");
            _currentState.InQuery = _currentState.IsStart;
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
            if (_currentState.Operations.Contains(operation))
            {
                throw new InvalidOperationException($"{operation} has alread been added to query.");
            }
            if (!operation.StartsWith("$"))
            {
                throw new InvalidOperationException($"{operation} is not a valid OData operation.");
            }
            _currentState.Operations.Add(operation);
            _currentState.CurrentOperation = operation;
            if (!_currentState.IsStart)
            {
                _uri.Append(_currentState.InSubQuery ? ';' : '&');
            }
            _currentState.IsStart = false;
            _uri.Append(operation);
            return this;
        }

        internal ODataQuery<T> AssertCurrentOperation(string operation)
        {
            if (_currentState.CurrentOperation != operation)
            {
                throw new InvalidExpressionException($"Expected {operation} but was in {_currentState.CurrentOperation}.");
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

        internal ODataQuery<T> AppendModifier<TV>(TV modifier)
        {
            _uri.Append(modifier);
            return this;
        }

        internal ODataQuery<T> StartSubQuery()
        {
            if (_currentState.IsStart)
            {
                throw new InvalidExpressionException("Cannot start a sub-query at the start of another query.");
            }
            _currentState.IsStart = true;
            _state.Push(_currentState);
            _currentState = new State();
            _currentState.InSubQuery = true;
            _uri.Append('(');
            return this;
        }

        internal ODataQuery<T> EndSubQuery()
        {
            if (_currentState.IsStart)
            {
                throw new InvalidExpressionException("Cannot create an empty sub-query.");
            }
            if (!_currentState.InSubQuery)
            {
                throw new InvalidExpressionException("Cannot end a sub-query when no sub-query started.");
            }
            _currentState = _state.Pop();
            if (_currentState == null)
            {
                throw new InvalidExpressionException("Sub-query start and end mismatch.");
            }
            _currentState.InSubQuery = _state.Any();
            return this;
        }

        public ODataQuery<T> AssertNotInQuery()
        {
            throw new NotImplementedException();
        }

        public ODataQuery<T> AppendUriElement(string operation)
        {
            throw new NotImplementedException();
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
