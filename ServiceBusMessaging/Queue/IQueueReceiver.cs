using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public interface IQueueReceiver<T> where T : class
    { 
        void ReceiveAndProcessMessages();
        Task CloseQueueAsync();
    }
}