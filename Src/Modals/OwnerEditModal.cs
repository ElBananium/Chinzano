using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
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

        public override string Title => "Если вы не хотите менять, укажите -1";

        public override ModalComponentBuilder GetModalsComponent()
        {
           return new ModalComponentBuilder()
                .WithTextInput("Склад", "storage")
                .WithTextInput("Подготовлено к продаже", "deliverystorage");
        }

        public override async Task HandleModal(Dictionary<string, string> TextInputsValues, SocketModal modal)
        {
            long storage;
            long deliverystorage;

            if (!long.TryParse(TextInputsValues["storage"], out storage)) return;
            if (!long.TryParse(TextInputsValues["deliverystorage"], out deliverystorage)) return;

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


            await modal.DeferAsync();

        }

        public OwnerEditModal(IGenericRepository repo)
        {
            _repo = repo;
        }
    }
}
