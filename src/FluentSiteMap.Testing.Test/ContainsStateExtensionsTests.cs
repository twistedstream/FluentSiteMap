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

        private static object[] _primitiveAndSimpleObjectNotEqualCases =
            {
                // primitive types:

                // bool
                new object[] {true, false},
                new object[] {(bool?) true, (bool?) false},

                // byte
                new object[] {(byte) 1, (byte) 2},
                new object[] {(byte?) 1, (byte?) 2},

                // sbyte
                new object[] {(sbyte) 1, (sbyte) 2},
                new object[] {(sbyte?) 1, (sbyte?) 2},

                // Int16
                new object[] {(Int16) 1, (Int16) 2},
                new object[] {(Int16?) 1, (Int16?) 2},
                new object[] {(UInt16) 1, (UInt16) 2},
                new object[] {(UInt16?) 1, (UInt16?) 2},

                // Int32
                new object[] {1, 2},
                new object[] {(Int32?) 1, (Int32?) 2},
                new object[] {(UInt32) 1, (UInt32) 2},
                new object[] {(UInt32?) 1, (UInt32?) 2},

                // Int64
                new object[] {(Int64) 1, (Int64) 2},
                new object[] {(Int64?) 1, (Int64?) 2},
                new object[] {(UInt64) 1, (UInt64) 2},
                new object[] {(UInt64?) 1, (UInt64?) 2},

                // IntPtr
                new object[] {(IntPtr) 1, (IntPtr) 2},
                new object[] {(IntPtr?) 1, (IntPtr?) 2},
                new object[] {(UIntPtr) 1, (UIntPtr) 2},
                new object[] {(UIntPtr?) 1, (UIntPtr?) 2},

                // char
                new object[] {(char) 1, (char) 2},
                new object[] {(char?) 1, (char?) 2},
                new object[] {(char) 1, (char) 2},
                new object[] {(char?) 1, (char?) 2},

                // double
                new object[] {(double) 1, (double) 2},
                new object[] {(double?) 1, (double?) 2},
                new object[] {(double) 1, (double) 2},
                new object[] {(double?) 1, (double?) 2},

                // float (Single)
                new object[] {(float) 1, (float) 2},
                new object[] {(float?) 1, (float?) 2},
                new object[] {(float) 1, (float) 2},
                new object[] {(float?) 1, (float?) 2},

                // simple types:

                new object[] {"foo", "bar"},
            };

        [Test]
        [TestCaseSource("_primitiveAndSimpleObjectNotEqualCases")]
        public void ContainsState_should_fail_if_primative_or_simple_objects_are_not_equal(object actual, object expected)
        {
            var result = actual.ContainsState(expected);

            Assert.That(result.Success, Is.False);
            var expectedFailReason = string.Format("/: Actual value '{0}' is not equal to expected value '{1}'.",
                                                   actual,
                                                   expected);
            Assert.That(result.FailReason, Is.EqualTo(expectedFailReason));
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
            Assert.That(result.FailReason, Is.EqualTo("/2: Actual value 'foo' is not equal to expected value 'three'."));
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
            Assert.That(result.FailReason, Is.EqualTo("/Addresses/1: Actual value '456 Elm St.' is not equal to expected value '456 Elm Street'."));
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
            Assert.That(result.FailReason, Is.EqualTo("/Address/City: Actual value 'NYC' is not equal to expected value 'Minneapolis'."));
        }
    }
}
