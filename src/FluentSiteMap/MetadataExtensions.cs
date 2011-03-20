using System;
using System.Collections.Generic;

namespace FluentSiteMap
{
    /// <summary>
    /// Provides extension methods to help work with node metadata.
    /// </summary>
    public static class MetadataExtensions
    {
        /// <summary>
        /// Returns true if the specified metadata key exists and returns true.
        /// </summary>
        public static bool IsTrue(this IDictionary<string, object> metadata, string key)
        {
            if (metadata == null) throw new ArgumentNullException("metadata");
            if (key == null) throw new ArgumentNullException("key");

            return 
                metadata.ContainsKey(key) 
                && (metadata[key].Equals(true));
        }
    }
}
