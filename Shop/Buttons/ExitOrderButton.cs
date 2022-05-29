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

        public override ButtonBuilder GetButton()
        {
            return new ButtonBuilder() { Label = "Выйти", Style = ButtonStyle.Danger };
        }

        public override async Task OnButtonClicked(SocketMessageComponent arg, Dictionary<string, string> info)
        {

            await (arg.Channel as SocketGuildChannel).DeleteAsync();
        }

        public ExitOrderButton()
        {
        }
    }
}
