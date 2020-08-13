using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusMessaging.MessagingModels
{
    public class UpdateStocksDto
    {
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }

    }
}
