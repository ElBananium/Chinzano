using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Buttons;
using Middleware.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Buttons
{
    public class ManagerMenuBuilderBtn : ButtonBase
    {
        private MenuService _menuService;

        private DiscordSocketClient _client;

        private IConfiguration _config;

        public override ButtonBuilder GetButton()
        {
            return new ButtonBuilder() { Style = ButtonStyle.Success, Label = "Открыть меню менеджера" };
        }

        public override async Task OnButtonClicked(SocketMessageComponent arg, Dictionary<string, string> info)
        {
            await arg.DeferAsync();

            var guild = _client.GetGuild(ulong.Parse(_config["currentguildid"]));

            var channel = guild.GetTextChannel((ulong)arg.ChannelId);

            await channel.AddPermissionOverwriteAsync(arg.User, new OverwritePermissions(viewChannel: PermValue.Deny));



            var createdchannel = await guild.CreateTextChannelAsync($"Панель менеджера", x => {
                x.CategoryId = channel.CategoryId;
                });



            await createdchannel.AddPermissionOverwriteAsync(guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            await createdchannel.AddPermissionOverwriteAsync(arg.User, new OverwritePermissions(viewChannel: PermValue.Allow));
            


            var compbuilder = new ComponentBuilder();
            compbuilder.WithSelectMenu(_menuService.GetMenuByName("ManagerMenu"));
            await createdchannel.SendMessageAsync("Выберите тип", components: compbuilder.Build());

            


           
        }

        public ManagerMenuBuilderBtn(MenuService menuservice, DiscordSocketClient client, IConfiguration config)
        {
            _menuService = menuservice;
            _client = client;
            _config = config;
        }
    }
}
