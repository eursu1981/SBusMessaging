using Core.Data.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Products.Models
{
    public class ProductsDto
    {
        public int? ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int BrandId { get; set; }
       
        public string BrandName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string CategorytName { get; set; }
        [Required]
        public short ModelYear { get; set; }
        [Required]
        public decimal ListPrice { get; set; }


        public ProductsDto() { }
        public ProductsDto(Core.Domain.Entities.Products entity)
        {

            ProductId = entity.ProductId;
            ProductName = entity.ProductName;
            BrandId = entity.BrandId;
            BrandName = entity.Brand?.BrandName;
            CategoryId = entity.CategoryId;
            CategorytName = entity.Category?.CategoryName;
            ModelYear = entity.ModelYear;
            ListPrice = entity.ListPrice;
        }

        internal Core.Domain.Entities.Products ToEntity(Core.Domain.Entities.Products entity, ActionType action)
        {
            if (action == ActionType.Update)
            {
                entity.Brand = null;
                entity.Category = null;
            }

            entity.ProductId = ProductId.HasValue ? ProductId.Value : 0;
            entity.ProductName = ProductName;
            entity.CategoryId = CategoryId;
            entity.BrandId = BrandId;
            entity.ModelYear = ModelYear;
            entity.ListPrice = ListPrice;
            return entity;
        }


    }
}
