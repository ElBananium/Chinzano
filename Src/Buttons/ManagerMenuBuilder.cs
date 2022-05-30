using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware;
using Middleware.Buttons;
using Middleware.Menu;
using Src.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Buttons
{
    public class ManagerMenuBuilderBtn : ButtonBase
    {

        private DiscordSocketClient _client;

        private IConfiguration _config;

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Style = ButtonStyle.Success, Label = "Открыть меню менеджера" };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            await arg.DeferAsync();


            var channel = arg.Channel as SocketTextChannel;

            await channel.AddPermissionOverwriteAsync(arg.User, new OverwritePermissions(viewChannel: PermValue.Deny));



            var createdchannel = await Guild.CreateTextChannelAsync($"Панель менеджера", x => {
                x.CategoryId = channel.CategoryId;
                });



            await createdchannel.AddPermissionOverwriteAsync(Guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            await createdchannel.AddPermissionOverwriteAsync(arg.User, new OverwritePermissions(viewChannel: PermValue.Allow, sendMessages: PermValue.Deny));
            


            var compbuilder = new AdditionalComponentBuilder().WithSelectMenu<ManagerMenu>();
            await createdchannel.SendMessageAsync("Выберите тип", components: compbuilder.Build());

            


           
        }

        public ManagerMenuBuilderBtn(DiscordSocketClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }
    }
}
