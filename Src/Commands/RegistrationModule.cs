using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Buttons;
using Middleware.Menu;
using Middleware.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Commands
{
    public class RegistrationModule : ModuleBase<SocketCommandContext>
    {
        private ButtonService _buttonService;

        private MenuService _menuService;

        private IConfiguration _configuration;




        [Command("CreateRegMessage")]
        public async Task CreateResisrationChannelAndMessageAsync()
        {
            if (!Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator) return;

            await Context.Message.DeleteAsync();
                var btn = _buttonService.GetButtonByName("RegistrationBtn", null);

            var components = new ComponentBuilder().WithButton(btn);
                

                await Context.Channel.SendMessageAsync("Нажмите", components: components.Build());
        }

        [Command("CreateStockCategory")]
        public async Task CreateOwnerMenu()
        {
                if (!Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator) return;

            var category = await Context.Guild.CreateCategoryChannelAsync("Складской учет");

                var channel = await Context.Guild.CreateTextChannelAsync("Панель основателя", x => x.CategoryId = category.Id);
                var compbuilder = new ComponentBuilder();

                compbuilder.WithSelectMenu(_menuService.GetMenuByName("OwnerMenu"));

                await channel.SendMessageAsync("Выберите тип",components: compbuilder.Build());




            channel = await Context.Guild.CreateTextChannelAsync("Открыть панель менеджера", x=> x.CategoryId = category.Id);

            compbuilder = new ComponentBuilder();

            compbuilder.WithButton(_buttonService.GetButtonByName("ManagerMenuBuilderBtn", null)).Build();

            await channel.SendMessageAsync("Нажмите на кнопку", components: compbuilder.Build());


            channel = await Context.Guild.CreateTextChannelAsync("Крафт броников", x => x.CategoryId = category.Id);


            compbuilder = new ComponentBuilder();
            compbuilder.WithButton(_buttonService.GetButtonByName("BulletproofsCraftButton", null));

            await channel.SendMessageAsync("Просто укажите сколько вы хотите получить.", components: compbuilder.Build());




        }
        public RegistrationModule(ButtonService buttonservice, MenuService menuService, IConfiguration config)
        {
            _buttonService = buttonservice;
            _menuService = menuService;
            _configuration = config;
        }
    }
}
