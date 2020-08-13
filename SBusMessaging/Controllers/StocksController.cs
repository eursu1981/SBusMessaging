using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Data;
using Core.Data.Repository;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Production.Models;
using ServiceBusMessaging;
using ServiceBusMessaging.MessagingModels;

namespace Production.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
       private readonly IQueueReceiver<List<UpdateStocksDto>> _queueReceiver;

        public StocksController(IQueueReceiver<List<UpdateStocksDto>> queueReceiver)
        {
            _queueReceiver = queueReceiver;
        }

        [HttpGet("[action]")]
        public ActionResult RefreshStocks()
        {
            try
            {
                _queueReceiver.ReceiveAndProcessMessages();            
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
