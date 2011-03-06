﻿using System.Linq;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class DefaultFilterProviderTests
        : TestBase
    {
        [Test]
        public void GetFilters_should_return_the_expected_filters()
        {
            // Arrange
            IDefaultFilterProvider target = new DefaultFilterProvider();

            // Act
            var results = target.GetFilters().ToList();

            // Assert
            Assert.That(results.Count, Is.EqualTo(0));
        }
    }
}