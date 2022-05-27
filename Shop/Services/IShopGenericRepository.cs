using Data.TradeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services
{
    public interface IShopGenericRepository
    {
        public void AddExistRepository(TradeRepo repo);

        public IEnumerable<TradeRepo> GetAllRepositories();

        public TradeRepo GetRepositoryByName(string name);

    }
}
