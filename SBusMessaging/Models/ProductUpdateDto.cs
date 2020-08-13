using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Production.Models
{
    public class ProductUpdateDto 
    {
        [Required]
        public decimal ListPrice { get; set; }

        internal Core.Domain.Entities.Products ToProductEntity(Core.Domain.Entities.Products entity)
        {
            entity.ListPrice = ListPrice;
            return entity;        
        }

    }
}
