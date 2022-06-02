using Shop.Services.PlacedOrderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.BudgenManager
{
    public interface IBudgetManager
    {
        public Task OrderMadeProfit(PlacedOrder order);

        public Task ManagerWidthDrawSomeMoney(string managername, int count, string reason);
    }
}
