using Data.ShopPriceFiltersRepository;
using Data.TradeRepository;
using Discord;
using Discord.Commands;
using Middleware;
using Middleware.Menu;
using Shop.Menus;
using Shop.Services.ShopPriceHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Commands
{
    public class ShopModule : ModuleBase<SocketCommandContext>
    {
        private IShopPriceFiltersRepository _filtersrepo;
        private IGenericRepository _repos;
        private IShopPriceHandler _pricehandler;

        [Command("CreateShopCategory")]
        public async Task CreateShopCategory()
        {
            if (!Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator) return;

            var category = await Context.Guild.CreateCategoryChannelAsync("Магазин");

            var channel = await Context.Guild.CreateTextChannelAsync("Прайс-лист", x => x.CategoryId = category.Id);
            await channel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, new(viewChannel: PermValue.Allow, sendMessages: PermValue.Deny));

            var embeds = PriceListEmbedsBuilder.GetEmbeds(_filtersrepo, _repos.GetAllRepositories());

            foreach(var embed in embeds)
            {
                await channel.SendMessageAsync(embed: embed.Build());
            }
            
            using (StreamWriter sw = new("pricelistchannelid.txt"))
            {
                sw.Write(channel.Id);
            }

            channel = await Context.Guild.CreateTextChannelAsync("Заказать", x => x.CategoryId = category.Id);
            await channel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, new(sendMessages: PermValue.Deny));
            var compbuilder = new AdditionalComponentBuilder();

            compbuilder.WithSelectMenu<OrderMenu>();

            await channel.SendMessageAsync("Что вы хотите заказать?", components: compbuilder.Build());

            using (StreamWriter sw = new("takeorderchannelid.txt"))
            {
                sw.Write(channel.Id);
            }

            
        }

        public ShopModule(IShopPriceFiltersRepository filtersrepo, IGenericRepository repos, IShopPriceHandler pricehandler)
        {
            _filtersrepo = filtersrepo;
            _repos = repos;
            _pricehandler = pricehandler;
        }




    }
}
