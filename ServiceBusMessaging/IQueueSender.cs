using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    internal interface IQueueSender<T> where T : class
    {
        Task SendAsync(T item, Dictionary<string, object> properties);
    }
}