using System.Collections;
using System.Linq;

namespace FluentSiteMap.Testing
{
    /// <summary>
    /// Contains extension methods for helping to write FluentSiteMap unit tests.
    /// </summary>
    public static class ContainsStateExtensions
    {
        /// <summary>
        /// Determines of the actual object contains the state of the expected object.
        /// </summary>
        /// <param name="actual">
        /// The object whose state is being examined.
        /// </param>
        /// <param name="expected">
        /// The state to confirm within the <paramref name="actual"/> object.  
        /// This parameter can be any object; however, a useful application is to use an 
        /// anonymous type.
        /// </param>
        /// <remarks>
        /// This method is useful for state-based unit tests where you need to assert 
        /// the state of objects with complext structures.
        /// </remarks>
        public static ContainsStateResult ContainsState(this object actual, object expected)
        {
            return actual.ContainsState(expected, string.Empty);
        }

        private static ContainsStateResult ContainsState(this object actual, object expected, string location)
        {
            // check for object equality
            if (Equals(actual, expected))
                return new ContainsStateResult();

            // check for collection equality
            var actualEnumerable = actual as IEnumerable;
            var expectedEnumerable = expected as IEnumerable;
            if (actualEnumerable != null && expectedEnumerable != null)
            {
                var expectedEnumerator = expectedEnumerable.GetEnumerator();
                var actualEnumerator = expectedEnumerable.GetEnumerator();
                var index = 0;

                while (expectedEnumerator.MoveNext())
                {
                    // actual collection not big enough
                    if (!actualEnumerator.MoveNext())
                        return new ContainsStateResult(location,
                                                       "Actual collection is not as large as expected.  Size exceeded at index {1}.",
                                                       index);

                    // compare collection items
                    var result = actualEnumerator.Current.ContainsState(expectedEnumerator.Current,
                                                                        location + "/" + index);
                    if (!result.Success)
                        return result;

                    index++;
                }
            }

            // check for property equality
            if (actual != null && expected != null)
            {
                var actualProperties = actual.GetType().GetProperties().ToDictionary(p => p.Name);
                var expectedProperties = expected.GetType().GetProperties();

                foreach (var expectedProperty in expectedProperties)
                {
                    var propertyLocation = location + "/" + expectedProperty.Name;

                    // actual property doesn't exist
                    if (!actualProperties.ContainsKey(expectedProperty.Name))
                        return new ContainsStateResult(propertyLocation, 
                            "Actual property is missing.");

                    // compare property values
                    var actualProperty = actualProperties[expectedProperty.Name];
                    var actualValue = actualProperty.GetValue(actual, null);
                    var expectedValue = expectedProperty.GetValue(expected, null);

                    var result = actualValue.ContainsState(expectedValue,
                                                           propertyLocation);
                    if (!result.Success)
                        return result;
                }
            }

            // expected is not contained within actual
            return new ContainsStateResult(location,
                                           "Actual value '{1}' is not equal to expected value '{2}'.",
                                           actual,
                                           expected);
        }
    }
}
