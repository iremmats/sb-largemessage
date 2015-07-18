using System;

namespace Iremmats.ServiceBus.LargeMessage
{
    public class LargeMessageConfigurationException : Exception
    {
        public LargeMessageConfigurationException(string message) : base(message)
        {
        }
    }
}
