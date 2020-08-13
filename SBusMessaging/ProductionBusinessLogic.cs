using Core.Data.Repository;
using Core.Domain.Entities;
using ServiceBusMessaging;
using ServiceBusMessaging.MessagingModels;
using ServiceBusMessaging.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Production
{
    public class ProductionBusinessLogic :IProcess
    {
        readonly Dictionary<Type, Action<dynamic>> _receiveDictionary;
       // private readonly IRepositoryBase<Stocks> _stocksRepo;

        public ProductionBusinessLogic()
        {
           // _stocksRepo = stocksRepo;
            _receiveDictionary = new Dictionary<Type, Action<dynamic>>()
            {
                 { typeof(List<UpdateStocksDto>), RefreshStocksForProducts },

            };

        }

        public MessageResponseEnum Process<T>(T message) where T: class
        {
            _receiveDictionary[typeof(T)].Invoke(message);
            return MessageResponseEnum.Complete;

        }
        private void RefreshStocksForProducts(dynamic list)
        {
            using var context = new BikeStoresContext();
            List<Stocks> stocksToUpdate = new List<Stocks>();
            if (list is List<UpdateStocksDto> && ((List<UpdateStocksDto>)list).Any())
            {
                    IRepositoryBase<Stocks> _stocksRepo = new RepositoryBase<Stocks>(context);
                    foreach (var item in (List<UpdateStocksDto>)list)
                    {
                        var stock = _stocksRepo.GetAllNoTracking().FirstOrDefault(x => x.ProductId == item.ProductId && x.StoreId == item.StoreId);
                        if (stock != null)
                        {
                            stock.Quantity = stock.Quantity >= item.Quantity ? stock.Quantity - item.Quantity : 0;
                            stocksToUpdate.Add(stock);
                        }
                        else
                        {
                            throw new Exception("Cannnot Update Stocks for all products");
                        }
                    }

                    if (stocksToUpdate.Any())
                    {
                        _stocksRepo.UpdateRange(stocksToUpdate);
                         _stocksRepo.Save();
                    }
            }
            
        }
    }
}
