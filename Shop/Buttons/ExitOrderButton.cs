using Discord;
using Discord.WebSocket;
using Middleware.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Buttons
{
    public class ExitOrderButton : ButtonBase
    {

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Выйти", Style = ButtonStyle.Danger };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {

            await (arg.Channel as SocketGuildChannel).DeleteAsync();
        }

        public ExitOrderButton()
        {
        }
    }
}
