using ServiceBusMessaging.Utils;
using System;

namespace ServiceBusMessaging
{
    public interface IQueueReceiver<T> where T : class
    {
        void Receive(
                 Func<T, MessageResponseEnum> onProcess,
                 Action<Exception> onError,
                 Action onWait);
    }
}