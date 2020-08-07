using Core.Data;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Products.Models;
using ServiceBusMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production
{
    public static class ProductionBusinessLogic
    {
        #region SendQueue
        public static async Task Queue_Send(dynamic item)
        {
            var appSettings = AppSettingsJson.GetAppSettings();

            var settings = new QueueSettings(
                appSettings["SBusAzure:SBusConnectionString"],
               appSettings["SBusAzure:SBusQueueName"]);


            IQueueSender<dynamic> sender = new QueueSender<dynamic>(settings);
            await sender.SendAsync(item, new Dictionary<string, object>());

        }
        #endregion
    }
}
