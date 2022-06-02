using Newtonsoft.Json;
using Shop.Services.PlacedOrderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.PlacedOrderArchive
{
    public class PlacedOrderArchive : IPlacedOrderArchive
    {
        private static object FileLock = new object();

        public void AddToArchive(DateTime daywhenclosed, PlacedOrder order)
        {
            string filename = daywhenclosed.Day + "." + daywhenclosed.Month + "." + daywhenclosed.Year;
            lock (FileLock)
            {
                List<PlacedOrder> orders = new();
                if (!File.Exists("OrdersArchive/" + filename + ".json"))
                {
                    var file = File.Create("OrdersArchive/" + filename + ".json");

                    file.Dispose();
                }

                else
                {
                    string filestring;
                    using (var sr = new StreamReader("OrdersArchive/" + filename + ".json"))
                    {
                        filestring = sr.ReadToEnd();

                    }
                    orders = JsonConvert.DeserializeObject<List<PlacedOrder>>(filestring);
                }

                orders.Add(order);

                using(var sw = new StreamWriter("OrdersArchive/" + filename + ".json"))
                {
                    sw.Write(JsonConvert.SerializeObject(orders));
                }



            }
            

        }

        public IEnumerable<PlacedOrder> GetOrdersInDay(DateTime day)
        {
            var orders = new List<PlacedOrder>();
            lock (FileLock)
            {
                string filename = day.Day + "." + day.Month + "." + day.Year;
                if (File.Exists("OrdersArchive/" + filename + ".json"))
                {
                    string filestring;
                    using (var sr = new StreamReader("OrdersArchive / " + filename + ".json"))
                    {
                        filestring = sr.ReadToEnd();

                    }
                    orders = JsonConvert.DeserializeObject<List<PlacedOrder>>(filestring);
                }

            }

            return orders;
        }

        public PlacedOrderArchive()
        {
            if (!Directory.Exists("OrdersArchive")) Directory.CreateDirectory("OrdersArchive"); 


        }
    }
}
