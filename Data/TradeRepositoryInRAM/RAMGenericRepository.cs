using Data.TradeRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data.TradeRepositoryInRAM
{
    public class RAMGenericRepository : IGenericRepository
    {
        private Dictionary<string, RAMTradeRepository> _repos;

        private object SerializeRepoLock = new object();

        public RAMGenericRepository()
        {
            _repos = new();




        }

        public void DeserializeRepo()
        {

            if (!File.Exists("repositories.json"))
            {
                return;
            }


            string stringfromfile;
            using (StreamReader sr = new StreamReader("repositories.json"))
            {
                stringfromfile = sr.ReadToEndAsync().GetAwaiter().GetResult();
            }
            var repolist = JsonConvert.DeserializeObject<List<DeserilizeRamTradeObject>>(stringfromfile);

            if (repolist.Count() == 0) return;

            foreach (var repo in repolist)
            {
                var repotoadd = _repos[repo.Name];
                if (repotoadd == null) continue;
                repotoadd.Deposit(repo.Count + repo.ToTradeCount);

                repotoadd.ToTrade(repo.ToTradeCount);
            }
        }

        public TradeRepo AddNewRepository(string name, string publicname)
        {
            var traderepo = new RAMTradeRepository(publicname, name);
            _repos.Add(name, traderepo);

            return traderepo;
        }

        public IEnumerable<TradeRepo> GetAllRepositories()
        {
            return _repos.Values;
        }

        public TradeRepo GetRepositoryByName(string name)
        {
            Task.Run(Serialize);
            return _repos[name];
            
        }



        private void Serialize()
        {

            


            if (!File.Exists("repositories.json"))
            {
                File.Create("repositories.json");

            }
            
            lock (SerializeRepoLock)
            {
                var repos = _repos.Values.ToList();
                var serstring = JsonConvert.SerializeObject(repos);

                using (StreamWriter sw = new StreamWriter("repositories.json"))
                {
                    sw.Write(serstring);
                }
            }
        }


        

    }
}
