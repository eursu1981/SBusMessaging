using ServiceBusMessaging.Utils;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public interface IProcess
    {
        MessageResponseEnum Process<T>(T message) where T : class;
    }
}