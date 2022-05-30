using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware;
using Middleware.Buttons;
using Middleware.Menu;
using Middleware.Modals;
using Src.Buttons;
using Src.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Commands
{
    public class RegistrationModule : ModuleBase<SocketCommandContext>
    {

        private IConfiguration _configuration;




        [Command("CreateRegMessage")]
        public async Task CreateResisrationChannelAndMessageAsync()
        {
            if (!Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator) return;

            await Context.Message.DeleteAsync();

            var components = new AdditionalComponentBuilder().WithButton<RegistrationBtn>();
                

                await Context.Channel.SendMessageAsync("Нажмите", components: components.Build());
        }

        [Command("CreateStockCategory")]
        public async Task CreateOwnerMenu()
        {
                if (!Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator) return;

            var category = await Context.Guild.CreateCategoryChannelAsync("Складской учет");

                var channel = await Context.Guild.CreateTextChannelAsync("Панель основателя", x => x.CategoryId = category.Id);
                var compbuilder = new AdditionalComponentBuilder().WithSelectMenu<OwnerMenu>();

                await channel.SendMessageAsync("Выберите тип",components: compbuilder.Build());




            channel = await Context.Guild.CreateTextChannelAsync("Открыть панель менеджера", x=> x.CategoryId = category.Id);

            compbuilder = new AdditionalComponentBuilder().WithButton<ManagerMenuBuilderBtn>();


            await channel.SendMessageAsync("Нажмите на кнопку", components: compbuilder.Build());


            channel = await Context.Guild.CreateTextChannelAsync("Крафт броников", x => x.CategoryId = category.Id);


            compbuilder = new AdditionalComponentBuilder().WithButton<BulletproofsCraftButton>();

            await channel.SendMessageAsync("Просто укажите сколько вы хотите получить.", components: compbuilder.Build());




        }
        [Command("DeleteAllChannel")]
        public async Task DeleAll()
        {
            foreach(var channel in Context.Guild.Channels.AsEnumerable()){
                await channel.DeleteAsync();
            }


        }
        public RegistrationModule(IConfiguration config)
        {
            _configuration = config;
        }
    }
}
