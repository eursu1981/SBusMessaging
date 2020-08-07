using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public class QueueSender<T> : IQueueSender<T> where T : class
    {
        private QueueSettings settings;
        private QueueClient client;

        public QueueSender(QueueSettings settings)
        {
            this.settings = settings;
            Init();
        }

        public async Task SendAsync(T item, Dictionary<string, object> properties)
        {
            var json = JsonConvert.SerializeObject(item);
            var message = new Message(Encoding.UTF8.GetBytes(json));

            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    message.UserProperties.Add(prop.Key, prop.Value);
                }
            }

            await client.SendAsync(message);
        }

        private void Init()
        {
            client = new QueueClient(
                     this.settings.ConnectionString, this.settings.QueueName);
        }
    }
}