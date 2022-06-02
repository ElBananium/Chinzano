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

namespace Src.Buttons
{
    public class ManagerBudgetBtn : ButtonBase
    {
        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Снять деньги", Style = ButtonStyle.Danger };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("channelid", arg.ChannelId.ToString());
            var modal = AdditionalComponentBuilder.GetModal<ManagerWidthdrawBudgetModal>(dict);

            await arg.RespondWithModalAsync(modal.Build());
        }
    }
}
