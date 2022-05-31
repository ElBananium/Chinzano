using Data.TradeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ShopPriceFiltersRepository
{
    public interface IShopPriceFiltersRepository
    {

        public IEnumerable<ShopFilter> GetFilters(TradeRepo repo);

        public void AddFilter(TradeRepo repo, uint countbreakpoint, int price);



        public void RemoveFilter(TradeRepo repo, ShopFilter filter);
    }
}
