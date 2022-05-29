using Data.TradeRepository;
using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.PlacedOrderRepository
{
    public class PlacedOrderRepository : IPlacedOrderRepository
    {
        private static object LastOrderNumberLock = new object();

        public PlacedOrder CreateOrder(string TradeRepoName, int HowManyOrdered, string WhatTime, bool IsRecived, ulong ChannelId)
        {
            

            var order = new PlacedOrder() { HowManyOrdered = HowManyOrdered, WhatTime = WhatTime, IsRecived = IsRecived, TradeRepoName = TradeRepoName, Id = GetLastOrderNumber() + 1, ChannelId = ChannelId };

            order.IsPicked = false;
            order.WhosPickedId = 0;

            var file = File.Create(Directory.GetCurrentDirectory() + "/PlacedOrders/" + order.Id + ".json");

            using(StreamWriter sw = new StreamWriter(file))
            {
                sw.Write(JsonConvert.SerializeObject(order));
            }

            using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + "/PlacedOrders/ordersinfo.json"))
            {
                sw.Write(order.Id);
            }

            var result = new FilePlaceOrder(order);

            result.Serialize();

            return result;



        }



        public PlacedOrder GetOrder(int id)
        {
            string stringfromfile;
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/PlacedOrders/" + id + ".json"))
            {
                stringfromfile = sr.ReadToEndAsync().GetAwaiter().GetResult();
            }
            var order = JsonConvert.DeserializeObject<PlacedOrder>(stringfromfile);

            return new FilePlaceOrder(order);
        }

        private int GetLastOrderNumber()
        {
            string stringfromfile;
            lock (LastOrderNumberLock)
            {
                using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/PlacedOrders/ordersinfo.json"))
                {
                    stringfromfile = sr.ReadToEndAsync().GetAwaiter().GetResult();
                }

            }
            return int.Parse(stringfromfile);
        }

        public void DeleteOrder(int id)
        {
            File.Delete(Directory.GetCurrentDirectory() + "/PlacedOrders/" + id + ".json");
        }

        public PlacedOrderRepository()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/PlacedOrders")) Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/PlacedOrders");
            if (!File.Exists(Directory.GetCurrentDirectory() + "/PlacedOrders/ordersinfo.json"))
            {
                var file = File.Create(Directory.GetCurrentDirectory() + "/PlacedOrders/ordersinfo.json");
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(0);
                }
                file.Dispose();
            }

        }
    }
}
