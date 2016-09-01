using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Serialization.MsgPack;

namespace CacheManager.Core
{
    public static class MsgPackConfigurationBuilderExtensions
    {
        /// <summary>
        /// Configures the cache manager to use the <code>ProtoBuf</code> based cache serializer.
        /// </summary>
        /// <param name="part">The configuration part.</param>
        /// <returns>The builder instance.</returns>
        public static ConfigurationBuilderCachePart WithMsgPackSerializer(this ConfigurationBuilderCachePart part)
        {
            Core.Utility.Guard.NotNull<ConfigurationBuilderCachePart>(part, nameof(part));

            return part.WithSerializer(typeof(MsgPackSerializer));
        }
    }
}
