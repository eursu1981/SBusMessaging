using Core.Data.Utils;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales.Models
{
    public class OrderItemsDto
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount { get; set; }

        public OrderItemsDto() { }
        public OrderItemsDto(OrderItems entity)
        {
            OrderId = entity.OrderId;
            ItemId = entity.ItemId;
            ProductId = entity.ProductId;
            ProductName = entity.Product?.ProductName;
            Quantity = entity.Quantity;
            ListPrice = entity.ListPrice;
            Discount = entity.Discount;
        }
        internal OrderItems ToEntity(OrderItems entity, int orderId)
        {
            entity.OrderId = orderId;
            entity.ItemId = ItemId;
            entity.ProductId = ProductId;
            entity.Quantity = Quantity;
            entity.ListPrice = ListPrice;
            entity.Discount = Discount;
            return entity;
        }
    }
}
