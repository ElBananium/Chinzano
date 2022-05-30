using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Middleware.Modals;
using Src.Services.RepositoryLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Modals
{
    public class ManagerDepositModal : ModalBase
    {
        private IGenericRepository _repo;

        private IRepositoryLogger _replogger;

        public override string Title => "Положить";

        public override ModalComponentBuilder GetComponent()
        {
            return new ModalComponentBuilder()
                .WithTextInput("Сколько положить?", "storage");
        }

        public override async Task OnComponentExecuted(SocketModal modal)
        {
            long count;
            if (!long.TryParse(TextInputsValues["storage"], out count)) return;
            if (count <= 0) return;

            var repo = _repo.GetRepositoryByName(AdditionalInfo["repname"]);
            repo.Deposit(count);
            await _replogger.LogDeposit(modal.User, repo, count);
            await modal.DeferAsync();

            var messages = await modal.Channel.GetMessagesAsync(100).FlattenAsync();
            if (messages.Count() > 1)
            {
                await messages.First().DeleteAsync();
            }


        }

        public ManagerDepositModal(IGenericRepository repo, IRepositoryLogger replogger)
        {
            _repo = repo;
            _replogger = replogger;
        }
    }
}
