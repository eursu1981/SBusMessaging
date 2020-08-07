using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public interface IQueueSender<T> where T : class
    {
        Task SendAsync(T item, Dictionary<string, object> properties);
    }
}