using Discord;
using Discord.WebSocket;
using Middleware;
using Middleware.Buttons;
using Shop.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Buttons
{
    public class AddPriceFilterBtn : ButtonBase
    {
        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Добавить новую точку", Style = ButtonStyle.Success };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            await arg.RespondWithModalAsync(AdditionalComponentBuilder.GetModal<AddFilterModal>(AdditionalInfo).Build());
        }
    }
}
