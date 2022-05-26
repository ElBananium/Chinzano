using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.TradeRepository
{
    public interface IGenericRepository
    {
        public TradeRepo AddNewRepository(string name, string publicname);

        public TradeRepo GetRepositoryByName(string name);

        public IEnumerable<TradeRepo> GetAllRepositories();

        
    }
}
