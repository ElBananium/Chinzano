using GraphDrawing.GraphDrawer;
using Shop.Services.PlacedOrderArchive;
using Shop.Services.PlacedOrderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.GraphicsOrTableInfoHandler
{
    public class GraphicsOrTableInfoHandler : IGraphicsOrTableInfoHandler
    {
        private IPlacedOrderArchive _placedOrderArchive;




        public IEnumerable<SalesGraphPoint> GetProfitInfo(int mounth, int from, int to)
        {
            var list = new List<SalesGraphPoint>();
            for(int i = from; i <= to; i++)
            {
                var date = new DateTime(DateTime.Now.Year, mounth, i);

                var orders = _placedOrderArchive.GetOrdersInDay(date);

                int count = 0;
                foreach(var order in orders)
                {
                    count += order.Price;
                }

                list.Add(new SalesGraphPoint(i, count));
            }
            return list;
        }

        public IEnumerable<SalesGraphPoint> GetSalesInfo(string tradereponame, int mounth, int from, int to)
        {
            var list = new List<SalesGraphPoint>();
            for (int i = from; i <= to; i++)
            {
                var date = new DateTime(DateTime.Now.Year, mounth, i);

                var orders = _placedOrderArchive.GetOrdersInDay(date).Where(x => x.TradeRepoName == tradereponame);

                int count = 0;
                foreach (var order in orders)
                {
                    count += order.HowManyOrdered;
                }

                list.Add(new SalesGraphPoint(i, count));
            }
            return list;
        }

        public GraphicsOrTableInfoHandler(IPlacedOrderArchive placedOrderArchive)
        {
            _placedOrderArchive = placedOrderArchive;
        }
    }
}
