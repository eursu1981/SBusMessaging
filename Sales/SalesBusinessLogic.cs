using Core.Data;
using Microsoft.Azure.ServiceBus;
using ServiceBusMessaging;
using ServiceBusMessaging.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales
{
    public static class SalesBusinessLogic
    {

        #region ReceiveQueue
        public static void Queue_Receive<T>() where T: class
        {
            var appSettings = AppSettingsJson.GetAppSettings();

            var settings = new QueueSettings(
                appSettings["SBusAzure:SBusConnectionString"],
                appSettings["SBusAzure:SBusQueueName"]);

            IQueueReceiver<T> receiver = new QueueReceiver<T>(settings);
            receiver.Receive(
                message =>
                {
                    return MessageResponseEnum.Complete;
                },
                ex => { //do something 
                },
                () => { //"Waiting..."
                });
        }
        #endregion
    }
}
