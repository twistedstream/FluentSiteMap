using System.Collections.Generic;
using NUnit.Framework.Constraints;

namespace FluentSiteMap.Testing
{
    /// <summary>
    /// Contains helper members for working with the <see cref="ContainsStateExtensions.ContainsState"/> method.
    /// </summary>
    public static class ContainsState
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

        /// <summary>
        /// Generates an NUnit <see cref="Constraint"/> that invokes the 
        /// <see cref="ContainsStateExtensions.ContainsState"/> extension method inline 
        /// with an NUnit assertion.
        /// </summary>
        /// <param name="expected">
        /// The state that is expected to be contained within actual object.
        /// </param>
        public static Constraint With(object expected)
        {
            return new ContainsStateConstraint(expected);
        }
    }
}
