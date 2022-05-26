using Data.TradeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Services.CraftingService
{
    public class CraftingService : ICraftingService
    {
        private TradeRepo _materialRepo;

        private TradeRepo _resultRepo;

        private int _craftprice;

        public void Craft(int resultcount)
        {

            if (!CanYouCraft(resultcount)) return;

            _materialRepo.Withdraw(resultcount * _craftprice);

            _resultRepo.Deposit(resultcount);
            
        }

        public bool CanYouCraft(int count)
        {
            if (_resultRepo == null || _materialRepo == null) throw new NullReferenceException();

            return !(count < 0 || _materialRepo.Count - count * _craftprice < 0);
        }

        public  void Configure(TradeRepo materialrepo, TradeRepo resultRepo, int craftprice)
        {
            _materialRepo = materialrepo;

            _resultRepo = resultRepo;

            _craftprice = craftprice;
        }
    }
}
