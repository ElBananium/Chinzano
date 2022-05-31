using Data.ShopPriceFiltersRepository;
using Discord;
using Discord.WebSocket;
using Middleware.Modals;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Modals
{
    public class AddFilterModal : ModalBase
    {
        private IShopPriceFiltersRepository _shopPricefilters;
        private IShopGenericRepository _shopGenericRepository;

        public override string Title => "Добавить фильтр";

        public override ModalComponentBuilder GetComponent()
        {
            return new ModalComponentBuilder().WithTextInput("Со скольки изменяется", "count").WithTextInput("Какая цена будет", "price");
        }

        public override async Task OnComponentExecuted(SocketModal arg)
        {
            int price;
            uint count;

            if (!uint.TryParse(TextInputsValues["count"], out count)) return;

            if(!int.TryParse(TextInputsValues["price"], out price)) return;
            var repo = _shopGenericRepository.GetRepositoryByName(AdditionalInfo["reponame"]);
            _shopPricefilters.AddFilter(repo, count, price);

            await arg.DeferAsync();

        }

        public AddFilterModal(IShopPriceFiltersRepository shopPriceFilters, IShopGenericRepository shopGenericRepository)
        {
            _shopPricefilters = shopPriceFilters;
            _shopGenericRepository = shopGenericRepository;
        }
    }
}
