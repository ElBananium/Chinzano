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
    public class RegistrationBtn : ButtonBase
    {

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Сменить никнейм", Style = ButtonStyle.Primary };
        }

        public async override Task OnComponentExecuted(SocketMessageComponent arg)
        {
            var modal = AdditionalComponentBuilder.GetModal<RegistrationModal>();
            await arg.RespondWithModalAsync(modal.Build());
        }

    }
}
