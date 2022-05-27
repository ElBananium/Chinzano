using Discord;
using Discord.Commands;
using Middleware.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Commands
{
    public class ShopModule : ModuleBase<SocketCommandContext>
    {
        private MenuService _menuService;


        [Command("CreateShopCategory")]
        public async Task CreateShopCategory()
        {
            if (!Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator) return;

            var category = await Context.Guild.CreateCategoryChannelAsync("Магазин");

            var channel = await Context.Guild.CreateTextChannelAsync("Заказать", x => x.CategoryId = category.Id);
            var compbuilder = new ComponentBuilder();

            compbuilder.WithSelectMenu(_menuService.GetMenuByName("OrderMenu"));

            await channel.SendMessageAsync("Что вы хотите заказать?", components: compbuilder.Build());
        }


        public ShopModule(MenuService menuService)
        {
            _menuService = menuService;
        }

    }
}
