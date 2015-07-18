using System;
using System.Collections.Generic;
using Microsoft.ServiceBus.Messaging;
using System.IO;

namespace Iremmats.ServiceBus.LargeMessage
{
    interface ILargeMessageReference
    {
        BrokeredMessage CreateMessage(string body, string queue, string messageType, CacheType cacheType);
        BrokeredMessage CreateMessage(string body, string queue, string messageType, CacheType cacheType, string filename);
        BrokeredMessage CreateMessage(string body, string queue, string messageType, CacheType cacheType, string filename, Dictionary<string, string> metadata);

        BrokeredMessage CreateMessage(Stream body, string queue, string messageType, CacheType cacheType);
        BrokeredMessage CreateMessage(Stream body, string queue, string messageType, CacheType cacheType, string filename);
        BrokeredMessage CreateMessage(Stream body, string queue, string messageType, CacheType cacheType, string filename, Dictionary<string, string> metadata);

        BrokeredMessage CreateMessage(byte[] body, string queue, string messageType, CacheType cacheType);
        BrokeredMessage CreateMessage(byte[] body, string queue, string messageType, CacheType cacheType, string filename);
        BrokeredMessage CreateMessage(byte[] body, string queue, string messageType, CacheType cacheType, string filename, Dictionary<string, string> metadata);

        Stream RetrieveBodyAsStream(BrokeredMessage msg);

        String RetrieveBodyAsString(BrokeredMessage msg);

        byte[] RetrieveBodyAsBytes(BrokeredMessage msg);
    }
}
