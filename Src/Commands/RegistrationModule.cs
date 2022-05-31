using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

using Middleware;
using Middleware.Buttons;
using Middleware.Menu;
using Middleware.Modals;
using Newtonsoft.Json;
using Shop.Menus;
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
        public async Task CreateStockCategory()
        {
            if (!Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator) return;
            var category = await Context.Guild.CreateCategoryChannelAsync("Складской учет");
            await category.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, new(viewChannel: PermValue.Deny, sendMessages: PermValue.Deny));


            //notifyformanagerchannelid
            var notifychannel = await Context.Guild.CreateTextChannelAsync("Уведомления", x => x.CategoryId = category.Id);


            var channel = await Context.Guild.CreateTextChannelAsync("Открыть панель менеджера", x => x.CategoryId = category.Id);

            var compbuilder = new AdditionalComponentBuilder().WithButton<ManagerMenuBuilderBtn>();


            await channel.SendMessageAsync("Нажмите на кнопку", components: compbuilder.Build());


            channel = await Context.Guild.CreateTextChannelAsync("Крафт броников", x => x.CategoryId = category.Id);


            compbuilder = new AdditionalComponentBuilder().WithButton<BulletproofsCraftButton>();

            await channel.SendMessageAsync("Просто укажите сколько вы хотите получить.", components: compbuilder.Build());


            string configstring;
            using (StreamReader sr = new("botconfig.json"))
            {
                configstring = sr.ReadToEnd();
            }
            var configdict = JsonConvert.DeserializeObject<Dictionary<string, string>>(configstring);

            configdict["notifyformanagerchannelid"] = notifychannel.Id.ToString();


            using (StreamWriter sw = new StreamWriter("botconfig.json"))
            {
                sw.Write(JsonConvert.SerializeObject(configdict));
            }
            (_configuration as IConfigurationRoot).Reload();



        }
        [Command("CreateOwnerCategory")]
        public async Task CreateOwnerCategory()
        {

            if (!Context.Guild.GetUser(Context.User.Id).GuildPermissions.Administrator) return;

            var category = await Context.Guild.CreateCategoryChannelAsync("Раздел основателя");
            await category.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, new(viewChannel: PermValue.Deny, sendMessages: PermValue.Deny));

            var channel = await Context.Guild.CreateTextChannelAsync("Панель основателя", x => x.CategoryId = category.Id);
            var compbuilder = new AdditionalComponentBuilder().WithSelectMenu<OwnerMenu>();

            await channel.SendMessageAsync("Выберите тип", components: compbuilder.Build());

            var pricefilterchannel = await Context.Guild.CreateTextChannelAsync("Цены", x => x.CategoryId = category.Id);
            compbuilder = new AdditionalComponentBuilder().WithSelectMenu<PriceShopRepoMenu>();

            await pricefilterchannel.SendMessageAsync("Редактировать цены", components: compbuilder.Build());


            var logchannel = await Context.Guild.CreateTextChannelAsync("Логи", x => x.CategoryId = category.Id);
            var techlogchannel = await Context.Guild.CreateTextChannelAsync("Тех.Логи", x => x.CategoryId = category.Id);
            var archivechannel = await Context.Guild.CreateTextChannelAsync("Архив", x => x.CategoryId = category.Id);
            string configstring;
            using (StreamReader sr = new("botconfig.json"))
            {
                configstring = sr.ReadToEnd();
            }
            var configdict = JsonConvert.DeserializeObject<Dictionary<string, string>>(configstring);

            configdict["currentguildid"] = Context.Guild.Id.ToString();
            configdict["publiclogchannelid"] = logchannel.Id.ToString();
            configdict["techlogid"] = techlogchannel.Id.ToString();
            configdict["orderarchivechannelid"] = archivechannel.Id.ToString();

            using (StreamWriter sw = new StreamWriter("botconfig.json"))
            {
                sw.Write(JsonConvert.SerializeObject(configdict));
            }
            (_configuration as IConfigurationRoot).Reload();

        }

        [Command("Test")]

        public async Task Test()
        {
            int l = new Random().Next(100);
            for(int i = 0; i<10; i++)
            {
                await Context.Channel.SendMessageAsync("ss " + l);
                await Task.Delay(500);
            }
            
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
