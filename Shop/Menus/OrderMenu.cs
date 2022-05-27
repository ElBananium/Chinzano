using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Buttons;
using Middleware.Menu;
using Shop.Services;
using Shop.Services.OrderSessionRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Menus
{
    public class OrderMenu : MenuBase
    {
        private IShopGenericRepository _shopRepo;

        private DiscordSocketClient _client;

        private IConfiguration _config;
        private IOrderSessionRepository _orderSessionService;
        private ButtonService _btnservice;

        public override string PlaceHolder => null;

        public override int MinValue => 1;

        public override int MaxValue => 1;

        public override SelectMenuOptionBuilder[] GetSelectMenuFields()
        {
            var opt = new List<SelectMenuOptionBuilder>();
            opt.Add(new SelectMenuOptionBuilder("-", "notchoisen", isDefault: true));

            foreach(var repo in _shopRepo.GetAllRepositories())
            {
                opt.Add(new SelectMenuOptionBuilder(repo.PublicName, repo.Name, isDefault: false));
            }
            return opt.ToArray();
        }

        public override async Task HandleMenu(SocketMessageComponent modal)
        {
            await modal.DeferAsync();
            var guild = _client.GetGuild(ulong.Parse(_config["currentguildid"]));

            var category = guild.GetTextChannel(modal.Channel.Id).Category;

            var channel = await guild.CreateTextChannelAsync("Заказ", x => x.CategoryId = category.Id);

            
            await channel.AddPermissionOverwriteAsync(guild.EveryoneRole, new(viewChannel: PermValue.Deny));
            await channel.AddPermissionOverwriteAsync(modal.User, new(viewChannel: PermValue.Allow, sendMessages: PermValue.Allow));

            _orderSessionService.AddNewSession(modal.User.Id, channel.Id, modal.Data.Values.First());

            var components = new ComponentBuilder().WithButton(_btnservice.GetButtonByName("CloseOrderButton", null));

            await channel.SendMessageAsync("Сколько вам нужно?", components: components.Build());


        }

        public OrderMenu(IShopGenericRepository shopRepo, DiscordSocketClient client, IConfiguration config, IOrderSessionRepository sessionservice, ButtonService btnservice)
        {
            _shopRepo = shopRepo;
            _client = client;
            _config = config;
            _orderSessionService = sessionservice;
            _btnservice = btnservice;
        }

    }
}
