using System.Web.Routing;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class BaseNodeBuilderTests
        : TestBase
    {
        [Test]
        public void Instances_should_have_an_empty_list_of_filters()
        {
            // Arrange
            INodeBuilder target = new BaseNodeBuilder();

            // Act, Assert
            Assert.That(target.Filters.Count, Is.EqualTo(0));
        }

        [Test]
        public void Build_should_create_an_unpopulated_NodeModel()
        {
            // Arrange
            var context = new BuilderContext(new RequestContext());
            
            INodeBuilder target = new BaseNodeBuilder();

            // Act
            var result = target.Build(context);
            
            // Assert
            Assert.That(result.Title, Is.Null);
            Assert.That(result.Description, Is.Null);
            Assert.That(result.Url, Is.Null);
            Assert.That(result.Children.Count, Is.EqualTo(0));
            Assert.That(result.Filters.Count, Is.EqualTo(0));
        }
    }
}