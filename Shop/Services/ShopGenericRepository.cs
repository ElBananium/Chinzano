using Data.TradeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services
{
    public class ShopGenericRepository : IShopGenericRepository
    {
        private Dictionary<string, TradeRepo> _repos;

        public ShopGenericRepository()
        {
            _repos = new();
        }

        public void AddExistRepository(TradeRepo repo)
        {
            
            _repos.Add(repo.Name, repo);
        }

        public IEnumerable<TradeRepo> GetAllRepositories()
        {
            return _repos.Values;
        }

        public TradeRepo GetRepositoryByName(string name)
        {
            return _repos[name];
        }
    }
}
