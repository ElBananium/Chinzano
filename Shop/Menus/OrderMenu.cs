using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware;
using Middleware.Buttons;
using Middleware.Menu;
using Shop.Buttons;
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
        private IConfiguration _config;
        private IOrderSessionRepository _orderSessionService;

        public override string PlaceHolder => null;

        public override int MinValue => 1;

        public override int MaxValue => 1;

        public override SelectMenuOptionBuilder[] GetComponent()
        {
            var opt = new List<SelectMenuOptionBuilder>();
            opt.Add(new SelectMenuOptionBuilder("-", "notchoisen", isDefault: true));

            foreach(var repo in _shopRepo.GetAllRepositories())
            {
                opt.Add(new SelectMenuOptionBuilder(repo.PublicName+" | Цена за штуку "+repo.PricePerItem, repo.Name, isDefault: false));
            }
            return opt.ToArray();
        }

        public override async Task OnComponentExecuted(SocketMessageComponent modal)
        {
            await modal.DeferAsync();
            var guild = Client.GetGuild(ulong.Parse(_config["currentguildid"]));

            var category = guild.GetTextChannel(modal.Channel.Id).Category;

            var channel = await guild.CreateTextChannelAsync("Заказ", x => x.CategoryId = category.Id);

            
            await channel.AddPermissionOverwriteAsync(guild.EveryoneRole, new(viewChannel: PermValue.Deny));
            await channel.AddPermissionOverwriteAsync(modal.User, new(viewChannel: PermValue.Allow, sendMessages: PermValue.Allow));

            _orderSessionService.AddNewSession(modal.User.Id, channel.Id, modal.Data.Values.First());

            var components = new AdditionalComponentBuilder().WithButton<CloseOrderButton>();

            await channel.SendMessageAsync("Сколько вам нужно?", components: components.Build());


        }

        public OrderMenu(IShopGenericRepository shopRepo,IConfiguration config, IOrderSessionRepository sessionservice)
        {
            _shopRepo = shopRepo;
            _config = config;
            _orderSessionService = sessionservice;
        }

    }
}
