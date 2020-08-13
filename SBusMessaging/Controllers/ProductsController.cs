using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Data.Repository;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Production.Models;
using Products.Models;
using ServiceBusMessaging;

namespace Production.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryBase<Core.Domain.Entities.Products> _productsRepo;
        private readonly IProcess _productionBusinessLogic;


        public ProductsController(IRepositoryBase<Core.Domain.Entities.Products> productsRepo, IRepositoryBase<Stocks> stocksRepo, IProcess productionBusinessLogic)
        {
            _productsRepo = productsRepo;
            _productionBusinessLogic = productionBusinessLogic;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> UpdatePriceForProduct([FromBody] ProductUpdateDto updateModel, int id) 
        {
            try
            {

                var entity = await _productsRepo.FindByIdAsync(id);
                var product = updateModel.ToProductEntity(entity);

                _productsRepo.Update(product);
                await _productsRepo.SaveChangesAsync();

                var item = new ProductsDto(product);

              //  ProductionBusinessLogic.Queue_Send(item).Wait();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update price!");
            }
        }

       


        //[HttpGet]
        //public async Task<ActionResult> Get()
        //{
        //    try
        //    {

        //        var product =await _productsRepo.FindByIdAsync(1);

        //        var item = new ProductsDto(product);

        //        ProductionBusinessLogic.Queue_Send(item).Wait();

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        
        //}

    }
}
