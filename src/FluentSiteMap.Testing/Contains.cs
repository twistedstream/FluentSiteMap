using System.Collections.Generic;

namespace FluentSiteMap.Testing
{
    /// <summary>
    /// Contains helper members for working with the <see cref="ContainsStateExtensions.ContainsState"/> method.
    /// </summary>
    public static class Contains
    {
        /// <summary>
        /// Specifies that an expected object is null.
        /// </summary>
        public static object Null
        {
            get { return null; }
        }

        /// <summary>
        /// Specifies that an expected object is an empty collection.
        /// </summary>
        public static IEnumerable<object> EmptyCollection
        {
            get
            {
                yield break;
            }
        }
    }
}
