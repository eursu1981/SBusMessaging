using Core.Data;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceBusMessaging.Utils;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public class QueueReceiver<T> : IQueueReceiver<T> where T : class
    {
        private QueueSettings _settings;
        private readonly IProcess _processData;
        private readonly ILogger _logger;
        private QueueClient _queueClient;

        public QueueReceiver(IProcess processData, ILogger<QueueReceiver<T>> logger)
        {
            var appSettings = AppSettingsJson.GetAppSettings();

            _settings = new QueueSettings(
                appSettings["SBusAzure:SBusConnectionString"],
               appSettings["SBusAzure:SBusQueueName"]);

            _processData = processData;
            _logger = logger;
            _queueClient = new QueueClient(
                   _settings.ConnectionString, _settings.QueueName);
        }

        public void ReceiveAndProcessMessages()
        {            
                var options = new MessageHandlerOptions(e =>
            {
                _logger.LogError(e.Exception, "Message handler encountered an exception");
                return Task.CompletedTask;
            })
                {
                    AutoComplete = false,
                    MaxAutoRenewDuration = TimeSpan.FromMinutes(1)
                };

                _queueClient.RegisterMessageHandler(ProcessMessageAsync, options);
        }

        public async Task CloseQueueAsync()
        {
            await _queueClient.CloseAsync();
        }

        private async Task ProcessMessageAsync(Message message, CancellationToken token) 
        {
            try
            {
                T item = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
                var result = _processData.Process(item);

                if (result == MessageResponseEnum.Complete)
                    await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
                else
                if (result == MessageResponseEnum.Abandon)
                    await _queueClient.AbandonAsync(message.SystemProperties.LockToken);
                else
                if (result == MessageResponseEnum.Dead)
                    await _queueClient.DeadLetterAsync(message.SystemProperties.LockToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Message handler encountered an exception");
                await _queueClient.DeadLetterAsync(message.SystemProperties.LockToken);

            }
        }
    }
}
