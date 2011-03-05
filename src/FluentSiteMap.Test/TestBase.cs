using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Impl;

namespace FluentSiteMap.Test
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
        }

        /// <summary>
        /// Method called after each test in a text fixture is executed.
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
        }

        /// <summary>
        /// Creates a <see cref="RequestContext"/> 
        /// that can accept MVC routing operations during a test.
        /// </summary>
        protected RequestContext MockRequestContextForRouting()
        {
            var httpRequest = MockRepository.GenerateStub<HttpRequestBase>();
            httpRequest
                .Stub(r => r.AppRelativeCurrentExecutionFilePath)
                .Return("~/some-url");
            httpRequest
                .Stub(r => r.PathInfo)
                .Return(string.Empty);

            var httpResponse = MockRepository.GenerateStub<HttpResponseBase>();
            httpResponse
                .Stub(r => r.ApplyAppPathModifier(Arg<string>.Is.Anything))
                .Do((Func<string, string>)(p => p));

            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext
                .Stub(c => c.Request)
                .Return(httpRequest);
            httpContext
                .Stub(c => c.Response)
                .Return(httpResponse);

            return new RequestContext
                       {
                           RouteData = RouteTable.Routes.GetRouteData(httpContext),
                           HttpContext = httpContext
                       };
        }
    }
}
