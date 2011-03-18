using System.Collections.Generic;
using System.Web.Routing;
using Rhino.Mocks;

namespace FluentSiteMap.Test.Builders
{
    public abstract class NodeBuilderTestBase
        : TestBase
    {
        public BuilderContext Context { get; set; }

        public INodeBuilder InnerBuilder { get; set; }

        public Node InnerNode { get; set; }

        public override void Setup()
        {
            base.Setup();

            Context = new BuilderContext(new RequestContext());

            InnerNode = new Node(new List<INodeFilter>());

            InnerBuilder = MockRepository.GenerateStub<INodeBuilder>();
            InnerBuilder
                .Stub(i => i.Build(Context))
                .Return(InnerNode);
        }
    }
}