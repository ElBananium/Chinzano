using Data.ShopPriceFiltersRepository;
using Data.TradeRepository;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public static class PriceListEmbedsBuilder
    {

        public static IEnumerable<EmbedBuilder> GetEmbeds(IShopPriceFiltersRepository filterrepo, IEnumerable<TradeRepo> tradeRepos)
        {
            
                var embedlist = new List<EmbedBuilder>();
                foreach (var repo in tradeRepos)
                {

                var filterslist = filterrepo.GetFilters(repo);
                if (filterslist == null) continue;    

                var filters = filterslist.OrderBy(x => x.CountBreakPoint).ToArray();
                if (filters[0].CountBreakPoint != 1 || filters.Length < 2)
                    {
                        continue;
                    }
                    var embed = new EmbedBuilder() { Title = repo.PublicName, Color = Color.Blue };
                    for (int i = 1; i < filters.Count(); i++)
                    {
                        embed.AddField($"От {filters[i-1].CountBreakPoint.ToString()} до {filters[i].CountBreakPoint.ToString()}", $"Цена :{filters[i-1].Price}");
                    }
                    embed.AddField($"Если больше {filters[filters.Count() - 1].CountBreakPoint.ToString()}", $"Цена : {filters[filters.Count() - 1].Price}");
                    embedlist.Add(embed);

                    


                }
            return embedlist;

        }
    }
}
