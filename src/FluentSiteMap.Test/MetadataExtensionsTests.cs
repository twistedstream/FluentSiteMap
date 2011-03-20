using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class MetadataExtensionsTests
        : TestBase
    {
        [Test]
        public void IsTrue_should_require_a_metadata_collection()
        {
            // Arrange
            IDictionary<string, object> metadata = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(
                () => metadata.IsTrue("foo"));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("metadata"));
        }

        [Test]
        public void IsTrue_should_require_a_key()
        {
            // Arrange
            var metadata = new Dictionary<string, object>();

            // Act
            var ex = Assert.Throws<ArgumentNullException>(
                () => metadata.IsTrue(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void IsTrue_should_return_false_if_the_key_does_not_exist()
        {
            // Arrange
            var metadata = new Dictionary<string, object>
                               {
                                   {"bar", true}
                               };

            // Act
            var result = metadata.IsTrue("foo");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsTrue_should_return_false_if_the_value_is_not_true()
        {
            // Arrange
            var metadata = new Dictionary<string, object>
                               {
                                   {"foo", "bar"}
                               };

            // Act
            var result = metadata.IsTrue("foo");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsTrue_should_return_true_if_the_metadata_key_exists_and_has_a_value_of_true()
        {
            // Arrange
            var metadata = new Dictionary<string, object>
                               {
                                   {"foo", true}
                               };

            // Act
            var result = metadata.IsTrue("foo");

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
