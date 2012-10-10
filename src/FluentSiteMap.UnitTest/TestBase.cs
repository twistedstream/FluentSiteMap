﻿using System;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Impl;

namespace FluentSiteMap.UnitTest
{
    /// <summary>
    /// Base class for all test classes.
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>
        /// Method called once before all the tests in a given test fixture are executed.
        /// </summary>
        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
            //write out Rhino logs for each test
            RhinoMocks.Logger = new TextWriterExpectationLogger(Console.Out);
        }

        /// <summary>
        /// Method called once after all of the tests in a given test fixture are executed.
        /// </summary>
        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
        }

        /// <summary>
        /// Method called before each test in a text fixture is executed.
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            RouteTable.Routes.Clear();
            SiteMapHelper.InjectRootNode(null);
            SiteMapHelper.InjectCurrentNode(null);
            SiteMapHelper.InjectHttpContext(null);
            SiteMapHelper.ClearCoordinator();
            SiteMapHelper.InjectRecursiveNodeFilter(null);
            SiteMapHelper.InjectDefaultFilterProvider(null);
        }

        /// <summary>
        /// Method called after each test in a text fixture is executed.
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
        }
    }
}
