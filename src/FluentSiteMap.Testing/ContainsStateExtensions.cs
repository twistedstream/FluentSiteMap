using System;
using System.Collections;
using System.Collections.Generic;
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
            return actual.ContainsState(expected, LocationDelimiter);
        }

        private const string LocationDelimiter = "/";

        private static readonly HashSet<Type> SimpleTypes = new HashSet<Type>
                                                                {
                                                                    typeof (string)
                                                                };

        private static ContainsStateResult ContainsState(this object actual, object expected, string location, Type actualType = null)
        {
            // check for object equality
            if (Equals(actual, expected))
                return new ContainsStateResult();

            // only continue more complex examination if both values are not null
            if (actual != null && expected != null)
            {
                if (actualType == null)
                    actualType = actual.GetType();
                var expectedType = expected.GetType();

                // only continue more complex examination if object is not a primitive or simple type
                if (!expectedType.IsPrimitive
                    &&
                    // Nullable of primitive type
                    !(expectedType.IsGenericType && Equals(expectedType.GetGenericTypeDefinition(), typeof (Nullable<>)) &&
                      expectedType.GetGenericArguments()[0].IsPrimitive)
                    &&
                    // one of the simple types
                    !SimpleTypes.Contains(expectedType))
                {

                    // check for collection equality
                    if (!(actual is string) && !(expected is string))
                    {
                        var actualEnumerable = actual as IEnumerable;
                        var expectedEnumerable = expected as IEnumerable;
                        if (actualEnumerable != null && expectedEnumerable != null)
                        {
                            var actualEnumerator = actualEnumerable.GetEnumerator();
                            var expectedEnumerator = expectedEnumerable.GetEnumerator();
                            var index = 0;

                            while (expectedEnumerator.MoveNext())
                            {
                                // actual collection not big enough
                                if (!actualEnumerator.MoveNext())
                                    return new ContainsStateResult(location,
                                                                   "Actual collection (size = {0}) is smaller than expected collection.",
                                                                   index);

                                // compare collection items
                                var result = actualEnumerator.Current.ContainsState(expectedEnumerator.Current,
                                                                                    location.Append(index));
                                if (!result.Success)
                                    return result;

                                index++;
                            }

                            // make sure actual collection doesn't contain any more items
                            if (actualEnumerator.MoveNext())
                                return new ContainsStateResult(location,
                                                               "Actual collection is larger than expected collection (size = {0}).",
                                                               index);

                            // collections were equal
                            return new ContainsStateResult();
                        }
                    }

                    // check for property equality
                    var actualProperties = actualType.GetProperties().ToDictionary(p => p.Name);
                    var expectedProperties = expectedType.GetProperties();

                    foreach (var expectedProperty in expectedProperties)
                    {
                        // actual property doesn't exist
                        if (!actualProperties.ContainsKey(expectedProperty.Name))
                            return new ContainsStateResult(location,
                                                           "Expected property '{0}' is missing in actual object.",
                                                           expectedProperty.Name);

                        // compare property values
                        var actualProperty = actualProperties[expectedProperty.Name];
                        var actualValue = actualProperty.GetValue(actual, null);
                        var expectedValue = expectedProperty.GetValue(expected, null);

                        var result = actualValue.ContainsState(expectedValue,
                                                               location.Append(expectedProperty.Name),
                                                               actualProperty.PropertyType);
                        if (!result.Success)
                            return result;
                    }

                    // properties were equal
                    return new ContainsStateResult();
                }
            }

            // expected is not contained within actual
            return new ContainsStateResult(location,
                                           "Actual value {0} is not equal to expected value {1}.",
                                           actual.FailMessageFormatted(),
                                           expected.FailMessageFormatted());
        }

        private static string Append(this string location, object value)
        {
            return string.Concat(location,
                                 location.EndsWith(LocationDelimiter) ? string.Empty : LocationDelimiter,
                                 value);
        }

        private static string FailMessageFormatted(this object value)
        {
            if (value == null)
                return "{null}";

            var stringValue = value as string;
            if (stringValue != null)
                return "'" + stringValue + "'";

            return value.ToString();
        }
    }
}
