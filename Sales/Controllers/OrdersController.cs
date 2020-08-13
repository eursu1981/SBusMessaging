using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Data.Repository;
using Core.Data.Utils;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Sales.Models;
using ServiceBusMessaging;
using ServiceBusMessaging.MessagingModels;

namespace Sales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IRepositoryBase<Orders> _ordersRepo;
        private readonly IQueueSender<List<UpdateStocksDto>> _queueSender;

        public OrdersController(IRepositoryBase<Orders> ordersRepo, IQueueSender<List<UpdateStocksDto>> queueSender)
        {
            _ordersRepo = ordersRepo;
            _queueSender = queueSender;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
               // SalesBusinessLogic.Queue_Receive<Products>();

                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateOrder([FromBody] OrdersDto model)
        {
            try {
                if (!ModelState.IsValid)
                    return BadRequest();

                using (var transaction = _ordersRepo.BeginTransaction())
                {
                    Orders entity = new Orders();
                    var order = model.ToEntity(entity, ActionType.Insert);

                    var insertedOrder = _ordersRepo.Add(order);
                    await _ordersRepo.SaveChangesAsync();

                    if (model.OrderItems != null && model.OrderItems.Any())
                    {
                        var itemsList = new List<OrderItems>();
                        itemsList.AddRange(model.OrderItems.Select(x => x.ToEntity(new OrderItems(), insertedOrder.OrderId)));
                        await _ordersRepo.SaveChangesAsync();

                        var stocksToUpdate = new List<UpdateStocksDto>();
                        stocksToUpdate.AddRange(itemsList.Select(x => new UpdateStocksDto { StoreId = insertedOrder.StoreId, ProductId = x.ProductId, Quantity = x.Quantity }));

                        _queueSender.SendAsync(stocksToUpdate).Wait();
                    }
                    transaction.Commit();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update price!");
            }
        }


    }
}
