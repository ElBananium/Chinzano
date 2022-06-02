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

        private string _serializefilename;

        private object SerializeRepoLock = new object();

        public RAMGenericRepository(string serializefilename)
        {
            _serializefilename = serializefilename;
            _repos = new();




        }

        public void DeserializeRepo()
        {

            if (!File.Exists(_serializefilename))
            {
                return;
            }


            string stringfromfile;
            using (StreamReader sr = new StreamReader(_serializefilename))
            {
                stringfromfile = sr.ReadToEndAsync().GetAwaiter().GetResult();
            }
            var repolist = JsonConvert.DeserializeObject<List<DeserilizeRamTradeObject>>(stringfromfile);

            if (repolist==null) return;

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
            RAMTradeRepository value = null;
            if(!_repos.TryGetValue(name, out value)) return null;

            Task.Run(Serialize);
            return value;
            
        }



        private void Serialize()
        {

            if (!File.Exists(_serializefilename))
            {
                File.Create(_serializefilename);

            }
            
            lock (SerializeRepoLock)
            {
                var repos = _repos.Values.ToList();
                var serstring = JsonConvert.SerializeObject(repos);

                using (StreamWriter sw = new StreamWriter(_serializefilename))
                {
                    sw.Write(serstring);
                }
            }
        }


        

    }
}
