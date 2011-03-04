using System.Collections.Generic;
using System.Web.Routing;
using Rhino.Mocks;

namespace FluentSiteMap.Test.Builders
{
    public abstract class NodeBuilderTestBase
        : TestBase
    {
        public BuilderContext Context { get; set; }

        public INodeBuilder Inner { get; set; }

        public override void Setup()
        {
            base.Setup();

            Context = new BuilderContext(new RequestContext());
            Inner = MockRepository.GenerateStub<INodeBuilder>();
            Inner
                .Stub(i => i.Build(Context))
                .Return(new NodeModel(new List<INodeFilter>()));
        }
    }
}