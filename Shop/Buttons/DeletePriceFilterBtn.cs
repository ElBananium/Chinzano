using Discord;
using Discord.WebSocket;
using Middleware;
using Middleware.Buttons;
using Shop.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Buttons
{
    public class DeletePriceFilterBtn : ButtonBase
    {
        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Удалить фильтр", Style = ButtonStyle.Danger };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            
                var components = new AdditionalComponentBuilder().WithSelectMenu<DeleteFilterMenu>(AdditionalInfo).WithButton<DeleteThisMsgBtn>();
                await arg.Channel.SendMessageAsync("Какой фильтр удалить?", components: components.Build());

                await arg.DeferAsync();
            
        }
    }
}
