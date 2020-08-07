using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServiceBusMessaging.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public class QueueReceiver<T> : IQueueReceiver<T> where T : class
    {
        private QueueSettings settings;
        private QueueClient client;

        public QueueReceiver(QueueSettings settings)
        {
            this.settings = settings;
            Init();
        }

        private void Init()
        {
            client = new QueueClient(
                     this.settings.ConnectionString, this.settings.QueueName);
        }

        public void Receive(
                Func<T, MessageResponseEnum> onProcess,
                Action<Exception> onError,
                Action onWait)
        {
            var options = new MessageHandlerOptions(e =>
            {
                onError(e.Exception);
                return Task.CompletedTask;
            })
            {
                AutoComplete = false,
                MaxAutoRenewDuration = TimeSpan.FromMinutes(1)
            };

            client.RegisterMessageHandler(
                async (message, token) =>
                {
                    try
                    {
                        // Get message
                        var data = Encoding.UTF8.GetString(message.Body);
                        T item = JsonConvert.DeserializeObject<T>(data);

                        // Process message
                        var result = onProcess(item);

                        if (result == MessageResponseEnum.Complete)
                            await client.CompleteAsync(message.SystemProperties.LockToken);
                        else if (result == MessageResponseEnum.Abandon)
                            await client.AbandonAsync(message.SystemProperties.LockToken);
                        else if (result == MessageResponseEnum.Dead)
                            await client.DeadLetterAsync(message.SystemProperties.LockToken);

                        // Wait for next message
                        onWait();
                    }
                    catch (Exception ex)
                    {
                        await client.DeadLetterAsync(message.SystemProperties.LockToken);
                        onError(ex);
                    }
                }, options);
        }

    }

    }
