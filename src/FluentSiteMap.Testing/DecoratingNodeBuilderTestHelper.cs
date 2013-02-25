using System.Collections.Generic;
using System.Web.Routing;
using Rhino.Mocks;

namespace FluentSiteMap.Testing
{
    /// <summary>
    /// A helper object for testing classes that inherit from <see cref="DecoratingNodeBuilder"/>.
    /// </summary>
    public class DecoratingNodeBuilderTestHelper
    {
        /// <summary>
        /// Gets or sets the <see cref="BuilderContext"/> that can be used to pass into the
        /// <see cref="DecoratingNodeBuilder.Build"/> method.
        /// </summary>
        public BuilderContext Context { get; set; }

        /// <summary>
        /// Gets or sets the inner <see cref="INodeBuilder"/> that can be used as 
        /// the <see cref="INodeBuilder"/> instance that the builder under test is decorating.
        /// </summary>
        public INodeBuilder InnerBuilder { get; set; }

        /// <summary>
        /// Gets or sets the node returned by the inner <see cref="INodeBuilder"/> 
        /// when it's <see cref="INodeBuilder.Build"/> method is called.
        /// </summary>
        public Node InnerNode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecoratingNodeBuilderTestHelper"/> class.
        /// </summary>
        public DecoratingNodeBuilderTestHelper()
        {
            Context = new BuilderContext(new RequestContext());
            InnerNode = new Node(new List<INodeFilter>());
            InnerBuilder = MockRepository.GenerateStub<INodeBuilder>();
            
            InnerBuilder
                .Stub(b => b.Build(Arg<BuilderContext>.Is.Anything))
                .Return(InnerNode);
            
            var filters = new List<INodeFilter>();
            InnerBuilder
                .Stub(b => b.Filters)
                .Return(filters);
        }
    }
}
