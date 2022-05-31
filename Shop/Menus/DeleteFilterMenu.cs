using Data.ShopPriceFiltersRepository;
using Discord;
using Discord.WebSocket;
using Middleware.Menu;
using Newtonsoft.Json;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Menus
{
    public class DeleteFilterMenu : MenuBase
    {
        private IShopPriceFiltersRepository _shopPriceFiltersRepository;
        private IShopGenericRepository _shopGenericRepository;

        public override string PlaceHolder => "Выбранные и фильтры будут удалены";

        public override int MinValue => 1;

        public override int MaxValue => 1;


        public override IEnumerable<SelectMenuOptionBuilder> GetComponent()
        {

            var result = new List<SelectMenuOptionBuilder>();

            

            foreach (var menu in _shopPriceFiltersRepository.GetFilters(_shopGenericRepository.GetRepositoryByName(AdditionalInfo["reponame"])))
            {
                result.Add(new("Cо скольки " + menu.CountBreakPoint+" |  Цена:" + menu.Price, JsonConvert.SerializeObject(menu)));
            }
            return result;

        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            await arg.DeferAsync();

            await arg.Message.DeleteAsync();
            var repo = _shopGenericRepository.GetRepositoryByName(AdditionalInfo["reponame"]);

            _shopPriceFiltersRepository.RemoveFilter(repo,JsonConvert.DeserializeObject<ShopFilter>(arg.Data.Values.First()));
            
            
        }

        public DeleteFilterMenu(IShopPriceFiltersRepository shopPriceFilterRepository, IShopGenericRepository shopGenericRepository)
        {
            _shopPriceFiltersRepository = shopPriceFilterRepository;
            _shopGenericRepository = shopGenericRepository;

            
        }
    }
}
