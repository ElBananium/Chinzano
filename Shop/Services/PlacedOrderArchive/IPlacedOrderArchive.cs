using Shop.Services.PlacedOrderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.PlacedOrderArchive
{
    public interface IPlacedOrderArchive
    {
        public void AddToArchive(DateTime daywhenclosed, PlacedOrder order);


        public IEnumerable<PlacedOrder> GetOrdersInDay(DateTime day);
    }
}
