using Discord;
using Discord.WebSocket;
using Middleware.Buttons;
using Shop.Services.OrderSessionRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Buttons
{
    public class CloseOrderButton : ButtonBase
    {
        private IOrderSessionRepository _orderRepo;

        public override ButtonBuilder GetButton()
        {
            return new ButtonBuilder() { Label = "Отменить заказ", Style = ButtonStyle.Danger };
        }

        public override async Task OnButtonClicked(SocketMessageComponent arg, Dictionary<string, string> info)
        {
            _orderRepo.RemoveSession((ulong)arg.ChannelId);

            await (arg.Channel as SocketGuildChannel).DeleteAsync();
        }

        public CloseOrderButton(IOrderSessionRepository orderrepo)
        {
            _orderRepo = orderrepo;
        }
    }
}
