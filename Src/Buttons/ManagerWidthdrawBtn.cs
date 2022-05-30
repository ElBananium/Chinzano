using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Middleware;
using Middleware.Buttons;
using Middleware.Modals;
using Src.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Buttons
{
    public class ManagerWidthdrawBtn : ButtonBase
    {


        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Снять", Style = ButtonStyle.Danger };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            var modal = AdditionalComponentBuilder.GetModal<ManagerWidthDrawModal>(AdditionalInfo);

            await arg.RespondWithModalAsync(modal.Build());
        }


    }
}
