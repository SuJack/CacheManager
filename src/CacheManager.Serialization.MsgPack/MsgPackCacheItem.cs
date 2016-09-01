using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CacheManager.Serialization.MsgPack
{
    [Serializable]
    [DataContract]
    public class MsgPackCacheItem
    {
        [DataMember(Order = 0)]
        public DateTime CreatedUtc { get; set; }

        [DataMember(Order = 1)]
        public DateTime LastAccessedUtc { get; set; }

        [DataMember(Order = 2)]
        public ExpirationMode ExpirationMode { get; set; }

        [DataMember(Order = 3)]
        public TimeSpan ExpirationTimeout { get; set; }

        [DataMember(Order = 4)]
        public string Key { get; set; }

        [DataMember(Order = 5)]
        public string Region { get; set; }

        [DataMember(Order = 6)]
        public byte[] Value { get; set; }

        public static MsgPackCacheItem FromCacheItem<TValue>(CacheItem<TValue> item, byte[] value)
        {
            return new MsgPackCacheItem
            {
                CreatedUtc = item.CreatedUtc,
                LastAccessedUtc = item.LastAccessedUtc,
                ExpirationMode = item.ExpirationMode,
                ExpirationTimeout = item.ExpirationTimeout,
                Key = item.Key,
                Region = item.Region,
                Value = value
            };
        }

        public CacheItem<T> ToCacheItem<T>(object value)
        {
            var output = string.IsNullOrEmpty(Region) ?
                    new CacheItem<T>(Key, (T)value, ExpirationMode, ExpirationTimeout) :
                    new CacheItem<T>(Key, Region, (T)value, ExpirationMode, ExpirationTimeout);

            output.LastAccessedUtc = LastAccessedUtc;
            return output.WithCreated(CreatedUtc);
        }
    }
}
