using Core.Data.Utils;
using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Sales.Models
{
    public class OrdersDto
    {
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public byte OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }

        // public  Customers Customer { get; set; }
        // public  Staffs Staff { get; set; }
        //  public  Stores Store { get; set; }
        public List<OrderItemsDto> OrderItems { get; set; }

        public OrdersDto()
        {
            OrderItems = new List<OrderItemsDto>();
        }

        public OrdersDto(Orders entity) : this()
        {
            OrderId = entity.OrderId;
            CustomerId = entity.CustomerId;
            OrderStatus = entity.OrderStatus;
            OrderDate = entity.OrderDate;
            RequiredDate = entity.RequiredDate;
            ShippedDate = entity.ShippedDate;
            StoreId = entity.StoreId;
            StaffId = entity.StaffId;
            CustomerName = entity.Customer?.FirstName + entity.Customer?.LastName;
            StaffName = entity.Staff?.FirstName + entity.Staff?.LastName;
            StoreName = entity.Store?.StoreName;
            if (entity.OrderItems.Any())
                OrderItems.AddRange(entity.OrderItems.Select(x => new OrderItemsDto(x)).ToList());
        }

        internal Orders ToEntity(Orders entity, ActionType action)
        {
            if (action == ActionType.Update)
            {
                entity.Customer = null;
                entity.Staff = null;
                entity.Store = null;
            }
            entity.CustomerId = CustomerId;
            entity.OrderStatus = OrderStatus;
            entity.OrderDate = OrderDate != DateTime.MinValue ? OrderDate : DateTime.Now;
            entity.RequiredDate = RequiredDate != DateTime.MinValue ? RequiredDate : DateTime.Now.AddDays(5);
            entity.ShippedDate = ShippedDate;
            entity.StoreId = StoreId;
            entity.StaffId = StaffId;
            return entity;
        }

    }
}
