using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FluentSiteMap.Testing.Test
{
    [TestFixture]
    public class ContainsStateExtensionsTests
        : TestBase
    {
        [Test]
        public void ContainsState_should_succeed_if_objects_are_equal()
        {
            const string actual = "foo";
            const string expected = "foo";

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ContainsState_should_succeed_if_objects_are_both_null()
        {
            object actual = null;
            object expected = null;

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.True);
        }

        private static IEnumerable<object> SimpleObjectNotEqualCases
        {
            get
            {
                var cases = new[]
                                {
                                    new object[] {true, false},
                                    new object[] {(byte) 1, (byte) 2},
                                    new object[] {(sbyte) 1, (sbyte) 2},
                                    new object[] {(Int16) 1, (Int16) 2},
                                    new object[] {(UInt16) 1, (UInt16) 2},
                                    new object[] {1, 2},
                                    new object[] {(UInt32) 1, (UInt32) 2},
                                    new object[] {(Int64) 1, (Int64) 2},
                                    new object[] {(UInt64) 1, (UInt64) 2},
                                    new object[] {(IntPtr) 1, (IntPtr) 2},
                                    new object[] {(UIntPtr) 1, (UIntPtr) 2},
                                    new object[] {(char) 1, (char) 2},
                                    new object[] {(double) 1, (double) 2},
                                    new object[] {(float) 1, (float) 2},
                                    new object[] {"foo", "bar"},
                                };
                foreach (var @case in cases)
                {
                    // return case
                    yield return @case;

                    // return Nullable case if a value type
                    var caseType = @case.GetType();
                    if (caseType.IsValueType)
                        yield return typeof (Nullable<>).MakeGenericType(caseType);
                }
            }
        }

        [Test]
        [TestCaseSource("SimpleObjectNotEqualCases")]
        public void ContainsState_should_fail_if_simple_objects_are_not_equal(object actual, object expected)
        {
            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailReason, Is.EqualTo("/: Actual value is not equal to expected value."));
            Assert.That(result.Actual, Is.EqualTo(actual));
            Assert.That(result.Expected, Is.EqualTo(expected));
        }

        [Test]
        public void ContainsState_should_format_nulls_correctly_in_the_fail_message()
        {
            object actual = "foo";
            object expected = null;

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailReason, Is.EqualTo("/: Actual value is not equal to expected value."));
            Assert.That(result.Actual, Is.EqualTo("foo"));
            Assert.That(result.Expected, Is.Null);
        }

        [Test]
        public void ContainsState_should_fail_if_the_actual_collection_is_smaller_than_the_expected()
        {
            var actual = new List<string> { "one", "two" };
            var expected = new[] { "one", "two", "three" };

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailReason, Is.EqualTo("/: Actual collection (size = 2) is smaller than expected collection."));
        }

        [Test]
        public void ContainsState_should_fail_if_the_actual_collection_is_larger_than_the_expected()
        {
            var actual = new List<string> { "one", "two", "three" };
            var expected = new[] { "one", "two" };

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailReason, Is.EqualTo("/: Actual collection is larger than expected collection (size = 2)."));
        }

        [Test]
        public void ContainsState_should_fail_if_the_actual_collection_contains_an_element_different_than_expected()
        {
            var actual = new List<string> { "one", "two", "foo" };
            var expected = new[] { "one", "two", "three" };

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailReason, Is.EqualTo("/2: Actual value is not equal to expected value."));
            Assert.That(result.Actual, Is.EqualTo("foo"));
            Assert.That(result.Expected, Is.EqualTo("three"));
        }

        [Test]
        public void ContainsState_succeed_if_collections_are_equal()
        {
            var actual = new List<string> { "one", "two", "three" };
            var expected = new[] { "one", "two", "three" };

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ContainsState_should_fail_if_the_actual_object_is_missing_a_property_from_the_expected_object()
        {
            var actual = new {Name = "foo"};
            var expected = new {Name = "foo", Age = 42};

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailReason, Is.EqualTo("/: Expected property 'Age' is missing in actual object."));
        }

        [Test]
        public void ContainsState_should_succeed_if_the_actual_object_matches_all_properties_of_expected_object()
        {
            var actual = new {Name = "Bob", Age = 42};
            var expected = new {Name = "Bob", Age = 42};

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ContainsState_should_succeed_if_the_actual_object_matches_all_properties_of_expected_object_and_contains_additional_properties()
        {
            var actual = new {Name = "Bob", Age = 42, Phone = "555-1234"};
            var expected = new {Name = "Bob", Age = 42};

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.True);
        }

        [Test]
        public void ContainsState_should_fail_if_a_contained_collection_is_not_equal_to_the_expected()
        {
            var actual = new
                             {
                                 Name = "Bob",
                                 Age = 42,
                                 Addresses = new[]
                                                 {
                                                     "123 Main St.",
                                                     "456 Elm St.",
                                                 }
                             };
            var expected = new
                               {
                                   Name = "Bob",
                                   Age = 42,
                                   Addresses = new[]
                                                 {
                                                     "123 Main St.",
                                                     "456 Elm Street",
                                                 }
                               };

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailReason, Is.EqualTo("/Addresses/1: Actual value is not equal to expected value."));
            Assert.That(result.Actual, Is.EqualTo("456 Elm St."));
            Assert.That(result.Expected, Is.EqualTo("456 Elm Street"));
        }

        [Test]
        public void ContainsState_should_fail_if_a_contained_object_is_not_equal_to_the_expected()
        {
            var actual = new
                             {
                                 Name = "Bob",
                                 Age = 42,
                                 Address = new
                                               {
                                                   Street = "123 Main St.",
                                                   City = "NYC"
                                               }
                             };
            var expected = new
                               {
                                   Name = "Bob",
                                   Age = 42,
                                   Address = new
                                                 {
                                                     Street = "123 Main St.",
                                                     City = "Minneapolis"
                                                 }
                               };

            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.False);
            Assert.That(result.FailReason, Is.EqualTo("/Address/City: Actual value is not equal to expected value."));
            Assert.That(result.Actual, Is.EqualTo("NYC"));
            Assert.That(result.Expected, Is.EqualTo("Minneapolis"));
        }

        [Test]
        public void ContainsState_should_succeed_if_a_contained_object_is_typed_differently_through_a_property_than_the_actual_object_type()
        {
            var actual = new
                             {
                                 // The IBar.Baz property is only accessible via the IBar interface
                                 Bar = (IBar) new Foo("bing")
                             };
            var expected = new
                               {
                                   Bar = new
                                             {
                                                 Baz = "bing"
                                             }
                               };

            var result = actual.ContainsState(expected);
            Assert.That(result.Success, Is.True);
        }

        internal interface IBar
        {
            string Baz { get; }
        }

        internal class Foo
            : IBar
        {
            public Foo(string baz)
            {
                _baz = baz;
            }
            private readonly string _baz;
            string IBar.Baz { get { return _baz; } }
        }
    }
}
