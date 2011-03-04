﻿using System;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that configures the node to get its URL from a named MVC controller action.
    /// </summary>
    public class ActionNodeBuilder
        : DecoratingNodeBuilder
    {
        /// <summary>
        /// The key used to store the action name in the <see cref="BuilderContext"/> metadata.
        /// </summary>
        public const string ActionMetadataKey = "action";

        private readonly string _actionName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="actionName">
        /// The name of the MVC controller action.
        /// </param>
        public ActionNodeBuilder(INodeBuilder inner, string actionName)
            : base(inner)
        {
            if (actionName == null) throw new ArgumentNullException("actionName");

            _actionName = actionName;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the <see cref="BuilderContext"/> metadata value.
        /// </summary>
        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            context.SetMetadata(ActionMetadataKey, _actionName);
        }
    }
}