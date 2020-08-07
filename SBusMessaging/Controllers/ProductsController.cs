using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Data.Repository;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Models;

namespace Production.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepositoryBase<Core.Domain.Entities.Products> _productsRepo;

        public ProductsController(IRepositoryBase<Core.Domain.Entities.Products> productsRepo)
        {
            _productsRepo = productsRepo;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {

                var product =await _productsRepo.FindByIdAsync(1);

                var item = new ProductsDto(product);

                ProductionBusinessLogic.Queue_Send(item).Wait();

                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        
        }

    }
}
