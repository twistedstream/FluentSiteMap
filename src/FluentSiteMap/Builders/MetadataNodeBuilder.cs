using System;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that sets metadata in a node.
    /// </summary>
    public class MetadataNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly string _key;
        private readonly object _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="key">
        /// The metadata key.
        /// </param>
        /// <param name="value">
        /// The metadata value.
        /// </param>
        public MetadataNodeBuilder(INodeBuilder inner, string key, object value) 
            : base(inner)
        {
            if (key == null) throw new ArgumentNullException("key");

            _key = key;
            _value = value;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the node title.
        /// </summary>
        protected override void OnBuild(Node node, BuilderContext context)
        {
            node.Metadata[_key] = _value;
        }
    }
}