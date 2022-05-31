using Data.ShopPriceFiltersRepository;
using Data.TradeRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ShopPriceFilterRepoInJson
{
    public class ShopPriceFilterRepositoryInJson : IShopPriceFiltersRepository
    {
        private static object ShopPriceFileLock = new object();
        private Dictionary<string, List<ShopFilter>> _filterslist;

        public void AddFilter(TradeRepo repo, uint countbreakpoint, int price)
        {

            List<ShopFilter> filters = new List<ShopFilter>();
            if (!_filterslist.TryGetValue(repo.Name, out filters))
            {
                _filterslist.Add(repo.Name, new());
                filters = _filterslist[repo.Name];
            }

            filters.Add(new ShopFilter() { CountBreakPoint = countbreakpoint, Price = price, TradeRepoName = repo.Name });
            SaveFilterList();
        }

        public void RemoveFilter(TradeRepo repo, ShopFilter filter)
        {


            _filterslist[repo.Name].Remove(filter);
            SaveFilterList();
        }

        public IEnumerable<ShopFilter> GetFilters(TradeRepo repo)
        {
            List<ShopFilter> result = null;
            _filterslist.TryGetValue(repo.Name, out result);

            return result;
        }


        private void GetFilterList()
        {
            string file;
            lock (ShopPriceFileLock)
            {


                using (StreamReader sr = new("ShopPriceFilters.json"))
                {
                    file = sr.ReadToEnd();
                }
            }

            _filterslist = JsonConvert.DeserializeObject<Dictionary<string, List<ShopFilter>>>(file) ?? new Dictionary<string, List<ShopFilter>>();

        }

        private void SaveFilterList()
        {
            string file = JsonConvert.SerializeObject(_filterslist);
            lock (ShopPriceFileLock)
            {


                using (StreamWriter sw = new("ShopPriceFilters.json"))
                {
                    sw.Write(file);
                }
            }

            _filterslist = JsonConvert.DeserializeObject<Dictionary<string, List<ShopFilter>>>(file);
        }

        public ShopPriceFilterRepositoryInJson()
        {

            if (!File.Exists("ShopPriceFilters.json"))
            {
                var file = File.Create("ShopPriceFilters.json");
                file.Dispose();
            }

            _filterslist = new();
            GetFilterList();


        }
    }
}
