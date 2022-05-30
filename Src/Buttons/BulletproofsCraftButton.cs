using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware;
using Middleware.Buttons;
using Middleware.Modals;
using Src.Modals;
using Src.Services.CraftingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Buttons
{
    public class BulletproofsCraftButton : ButtonBase
    {



        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Скрафтить", Style = ButtonStyle.Primary };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
           


            await arg.RespondWithModalAsync(AdditionalComponentBuilder.GetModal<BulletproofsCraftModal>().Build());

        }

    }
}
