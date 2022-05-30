using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Menu;
using Middleware.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Modals
{
    public class OwnerEditModal : ModalBase
    {
        private IGenericRepository _repo;
        private IConfiguration _config;
        private MenuService _menuService;

        public override string Title => "Если вы не хотите менять, укажите -1";

        public override ModalComponentBuilder GetComponent()
        {
            return new ModalComponentBuilder()
                 .WithTextInput("Склад", "storage")
                 .WithTextInput("Подготовлено к продаже", "deliverystorage")
                 .WithTextInput("Цена за 1 штуку", "price");
        }

        public override async Task OnComponentExecuted(SocketModal modal)
        {
            long storage;
            long deliverystorage;
            int priceperitem;

            if (!long.TryParse(TextInputsValues["storage"], out storage)) return;
            if (!long.TryParse(TextInputsValues["deliverystorage"], out deliverystorage)) return;
            if (!int.TryParse(TextInputsValues["price"], out priceperitem)) return;

            var repos = _repo.GetRepositoryByName(AdditionalInfo["repname"]);

            if (storage != -1 && storage >= 0)
            {
                if (storage >= repos.Count) repos.Deposit(storage - repos.Count);
                else repos.Withdraw(repos.Count - storage);
            }
            if (deliverystorage != -1 && deliverystorage >= 0)
            {
                if (deliverystorage >= repos.ToTradeCount)
                {
                    repos.Deposit(deliverystorage - repos.ToTradeCount);
                    repos.ToTrade(deliverystorage - repos.ToTradeCount);
                }
                else
                {
                    repos.Traded(repos.ToTradeCount - deliverystorage);
                }
            }
            if(priceperitem != -1 && priceperitem >= 0)
            {
                repos.SetPricePerIten((uint)priceperitem);

                ulong channelid;
                using(StreamReader sr = new("takeorderchannelid.txt"))
                {
                    channelid = ulong.Parse(sr.ReadToEnd());
                }


                var channel = Client.GetGuild(ulong.Parse(_config["currentguildid"])).GetTextChannel(channelid);
                var msgs = await channel.GetMessagesAsync(1).FlattenAsync();

                await msgs.First().DeleteAsync();

                var compbuilder = new ComponentBuilder();

                compbuilder.WithSelectMenu(_menuService.GetComponentByName("OrderMenu"));

                await channel.SendMessageAsync("Что вы хотите заказать?", components: compbuilder.Build());


            }

            var messages = await modal.Channel.GetMessagesAsync(100).FlattenAsync();
            if (messages.Count() > 1)
            {
                await messages.First().DeleteAsync();
            }


            await modal.DeferAsync();

        }

        public OwnerEditModal(IGenericRepository repo, IConfiguration config, Middleware.Menu.MenuService menuService)
        {
            _repo = repo;
            _config = config;
            _menuService = menuService;
        }
    }
}
