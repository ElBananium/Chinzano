using Data.TradeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.ShopPriceHandler
{
    public interface IShopPriceHandler
    {
        //

        public int GetPrice(TradeRepo repo, int count);

    }
}
