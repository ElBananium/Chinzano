using Data.ShopPriceFiltersRepository;
using Data.TradeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.ShopPriceHandler
{
    public class ShopPriceHandler : IShopPriceHandler
    {
        private IShopPriceFiltersRepository _filters;

        public int GetPrice(TradeRepo repo, int count)
        {
            var filters = _filters.GetFilters(repo);
            if (filters == null) return 0;
            var resfilters = filters.OrderBy(x => x.CountBreakPoint);

            int resultprice = 0;
            foreach(var price in resfilters)
            {
                if(count >= price.CountBreakPoint)
                {
                    resultprice = price.Price;
                    continue;
                }
                break;
                
            }
            return resultprice;


        }

        public ShopPriceHandler(IShopPriceFiltersRepository filters)
        {
            _filters = filters;
        }
    }
}
