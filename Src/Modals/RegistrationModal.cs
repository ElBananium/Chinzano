using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Modals
{
    public class RegistrationModal : ModalBase
    {
        private IConfiguration _config;
        public override string Title => "Регистрация";

        public override ModalComponentBuilder GetComponent()
        {
            return new ModalComponentBuilder()
                .WithTextInput("Игровой никнейм", "gamenickname")
                .WithTextInput("Реальное имя", "realname");
        }

        public override async Task OnComponentExecuted(SocketModal modal)
        {
            var guildid = ulong.Parse(_config["currentguildid"]);
            var user = Client
                 .GetGuild(guildid).GetUser(modal.User.Id);

            await modal.DeferAsync();

            await user.ModifyAsync(x => x.Nickname = $"{TextInputsValues["gamenickname"].Trim()} | {TextInputsValues["realname"].Trim()}");



            
        }

        public RegistrationModal(IConfiguration config)
        {
            _config = config;
        }
    }
}
