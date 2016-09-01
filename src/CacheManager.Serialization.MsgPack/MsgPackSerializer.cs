using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;
using CacheManager.Core.Internal;
using MsgPack.Serialization;

namespace CacheManager.Serialization.MsgPack
{
    public class MsgPackSerializer : ICacheSerializer
    {
        private static readonly Type cacheItemType = typeof(MsgPackCacheItem);

        public MsgPackSerializer()
        {
        }

        public object Deserialize(byte[] data, Type target)
        {
            if (data == null)
            {
                return null;
            }
            MessagePackSerializer serializer = SerializationContext.Default.GetSerializer(target);
            using (MemoryStream stream = new MemoryStream(data))
            {
                return serializer.Unpack(stream);
            }
        }

        public CacheItem<T> DeserializeCacheItem<T>(byte[] value, Type valueType)
        {
            var item = (MsgPackCacheItem)Deserialize(value, cacheItemType);
            if (item == null)
            {
                throw new Exception("Unable to deserialize the CacheItem");
            }

            var cachedValue = Deserialize(item.Value, valueType);
            return item.ToCacheItem<T>(cachedValue);
        }

        public byte[] Serialize<T>(T value)
        {
            MessagePackSerializer<T> serializer = SerializationContext.Default.GetSerializer<T>();
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Pack(stream, value);
                return stream.ToArray();
            }
        }

        public byte[] SerializeCacheItem<T>(CacheItem<T> value)
        {
            var cachedValue = Serialize(value.Value);
            var pbCacheItem = MsgPackCacheItem.FromCacheItem(value, cachedValue);

            return Serialize(pbCacheItem);
        }
    }
}
