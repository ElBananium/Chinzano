using Discord;
using Discord.WebSocket;
using Middleware.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Buttons
{
    public class DeleteThisMsgBtn : ButtonBase
    {
        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Выход", Style = ButtonStyle.Danger };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            await arg.DeferAsync();
            await arg.Message.DeleteAsync();
        }
    }
}
