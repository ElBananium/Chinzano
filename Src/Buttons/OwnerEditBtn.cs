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
    public class OwnerEditBtn : ButtonBase
    {


        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Редактировать", Style = ButtonStyle.Primary };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            var modal = AdditionalComponentBuilder.GetModal<OwnerEditModal>(AdditionalInfo);

            await arg.RespondWithModalAsync(modal.Build());
        }

    }
}
