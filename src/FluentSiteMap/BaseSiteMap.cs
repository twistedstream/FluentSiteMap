﻿using System;
using System.Collections.Generic;

namespace FluentSiteMap
{
    /// <summary>
    /// The base class for all site maps.  
    /// </summary>
    /// <remarks>
    /// Consumers of <see cref="SiteMapHelper"/> should subclass this base class to 
    /// define a site map, setting the <see cref="Root"/> property in their constructor, 
    /// chaining additional builders using the fluent builder interface.
    /// </remarks>
    public abstract class BaseSiteMap
        : INodeBuilder, ISiteMap
    {
        /// <summary>
        /// Gets or sets the root <see cref="INodeBuilder"/> in the site map.
        /// </summary>
        /// <remarks>
        /// This property should be set in the constructor of the derrived class.
        /// </remarks>
        protected INodeBuilder Root { get; set; }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> instance that can be used as 
        /// the base instance for a chain of decorator <see cref="INodeBuilder"/> instances.
        /// </summary>
        protected INodeBuilder Node()
        {
            return new BaseNodeBuilder();
        }

        /// <summary>
        /// Implements the <see cref="ISiteMap.Build"/> method.
        /// </summary>
        public Node Build(BuilderContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            EnsureRoot();

            return Root.Build(context);
        }

        /// <summary>
        /// Implements the <see cref="INodeBuilder.Filters"/> property.
        /// </summary>
        public IList<INodeFilter> Filters
        {
            get
            {
                EnsureRoot();
                return Root.Filters;
            }
        }

        private void EnsureRoot()
        {
            if (Root == null)
                throw new InvalidOperationException(
                    "The Root property must be set in the subclass's constructor.");
        }
    }
}