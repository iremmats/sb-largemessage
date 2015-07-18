using System;

namespace Iremmats.ServiceBus.LargeMessage
{
    public class LargeMessage
    {
        public string BodyLocation { get; set; }
        public CacheType CacheType { get; set; }
        public Uri Uri { get; set; }
        public Uri SasUri { get; set; }
    }
}
