using System;

namespace ServiceBusMessaging
{
    public class QueueSettings
    {
        public QueueSettings(string connectionString, string queueName)
        {

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentNullException("queueName");

            ConnectionString = connectionString;
            QueueName = queueName;
        }

        public string ConnectionString { get; }
        public string QueueName { get; }
    }
}
