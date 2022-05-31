using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ShopPriceFiltersRepository
{
    public class ShopFilter : IEquatable<ShopFilter>
    {
        public string TradeRepoName { get; set; }

        public uint CountBreakPoint { get; set; }
        
        public int Price { get; set; }

        public bool Equals(ShopFilter? other)
        {
            return TradeRepoName == other.TradeRepoName && CountBreakPoint == other.CountBreakPoint && Price == other.Price;
        }
    }
}
