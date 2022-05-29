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
    public class ManagerWidthDrawModal : ModalBase
    {
        private IGenericRepository _repo;

        private IRepositoryLogger _replogger;
        public override string Title => "Снять";

        public override ModalComponentBuilder GetModalsComponent()
        {
            return new ModalComponentBuilder()
                .WithTextInput("Сколько снять?", "storage");
        }

        public override async Task HandleModal(Dictionary<string, string> TextInputsValues, SocketModal modal)
        {
            long count;
            if (!long.TryParse(TextInputsValues["storage"], out count)) return;
            if(count <= 0) return;

            var repo = _repo.GetRepositoryByName(AdditionalInfo["repname"]);

            repo.Withdraw(count);

            await modal.DeferAsync();
            await _replogger.LogWidthDraw(modal.User, repo, count);


            var messages = await modal.Channel.GetMessagesAsync(100).FlattenAsync();
            if (messages.Count() > 1)
            {
                await messages.First().DeleteAsync();
            }


        }

        public ManagerWidthDrawModal(IGenericRepository repo, IRepositoryLogger replogger)
        {
            _repo = repo;
            _replogger = replogger;
        }
    }
}
