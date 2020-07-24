using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBusMessaging.Utils
{
    public enum MessageResponseEnum
    {
        Complete,
        Abandon,
        Dead
    }
}
