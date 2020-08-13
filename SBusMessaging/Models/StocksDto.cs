using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Production.Models
{
    public class StocksDto
    {
        [Required]
        public int StoreId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int? Quantity { get; set; }

        internal Stocks ToStocksEntity(Stocks entity)
        {
            entity.Quantity = Quantity;
            return entity;
        }

        public StocksDto() { }
        public StocksDto(Stocks entity) 
        {
            StoreId = entity.StoreId;
            ProductId = entity.ProductId;
            Quantity = entity.Quantity ?? 0;
        }

    }
}
