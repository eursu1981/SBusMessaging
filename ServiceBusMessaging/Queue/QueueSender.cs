using Core.Data;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public class QueueSender<T> : IQueueSender<T> where T : class
    {
        private QueueSettings _settings;
        private QueueClient _queueClient;

        public QueueSender()
        {
            var appSettings = AppSettingsJson.GetAppSettings();

            _settings = new QueueSettings(
                appSettings["SBusAzure:SBusConnectionString"],
               appSettings["SBusAzure:SBusQueueName"]);

            _queueClient = new QueueClient(
                   _settings.ConnectionString, _settings.QueueName);
        }


        public async Task SendAsync(T item)
        {
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var message = new Message(Encoding.UTF8.GetBytes(json));

                await _queueClient.SendAsync(message);
            }
            finally
            {
                await _queueClient.CloseAsync();
            }
        }
    }
}