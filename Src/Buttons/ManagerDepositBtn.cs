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
    public class ManagerDepositBtn : ButtonBase
    {
        private ModalService _modalService;

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Положить", Style = ButtonStyle.Success };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            var modal = AdditionalComponentBuilder.GetModal<ManagerDepositModal>(AdditionalInfo);

            await arg.RespondWithModalAsync(modal.Build());
        }


    }
}
