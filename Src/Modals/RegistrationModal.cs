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

        public override ModalComponentBuilder GetModalsComponent()
        {
            return new ModalComponentBuilder()
                .WithTextInput("Игровое имя", "gamenickname")
                .WithTextInput("Игровая фамилия", "gamesurname")
                .WithTextInput("Реальное имя", "realname");
        }

        public override async Task HandleModal(Dictionary<string,string> TextInputsValues, SocketModal modal)
        {
            var guildid = ulong.Parse(_config["currentguildid"]);
            var user = Client
                 .GetGuild(guildid).GetUser(modal.User.Id);



             await user.ModifyAsync(x => x.Nickname = $"{TextInputsValues["gamenickname"]} {TextInputsValues["gamesurname"]} | {TextInputsValues["realname"]}");



            await modal.DeferAsync();
        }

        public RegistrationModal(IConfiguration config)
        {
            _config = config;
        }
    }
}
