using Data.TradeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Services.CraftingService
{
    public interface ICraftingService
    {
        public void Configure(TradeRepo materialrepo, TradeRepo resultRepo, int craftprice);

        public bool CanYouCraft(int count);
        public void Craft(int count);
    }
}
