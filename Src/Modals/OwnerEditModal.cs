using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware;
using Middleware.Menu;
using Middleware.Modals;
using Shop.Menus;
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

        public override string Title => "-1 если ничего не менять";

        public override ModalComponentBuilder GetComponent()
        {
            return new ModalComponentBuilder()
                 .WithTextInput("Склад", "storage")
                 .WithTextInput("Подготовлено к продаже", "deliverystorage");
        }

        public override async Task OnComponentExecuted(SocketModal modal)
        {
            long storage;
            long deliverystorage;
            int priceperitem;

            if (!long.TryParse(TextInputsValues["storage"], out storage)) return;
            if (!long.TryParse(TextInputsValues["deliverystorage"], out deliverystorage)) return;

            await modal.DeferAsync();

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

            var messages = await modal.Channel.GetMessagesAsync(100).FlattenAsync();
            if (messages.Count() > 1)
            {
                await messages.First().DeleteAsync();
            }


            

        }

        public OwnerEditModal(IGenericRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }
    }
}
