using System;

namespace Scrumfish.OData.Client.Common
{
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

        public static ODataQuery<T> CreateODataQuery<T>(bool isSubquery = true)
        {
            return new ODataQuery<T>(isSubquery);
        }
    }
}