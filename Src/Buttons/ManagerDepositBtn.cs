using Discord;
using Discord.WebSocket;
using Middleware.Buttons;
using Middleware.Modals;
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

        public override ButtonBuilder GetButton()
        {
            return new ButtonBuilder() { Label = "Положить", Style = ButtonStyle.Success };
        }

        public override async Task OnButtonClicked(SocketMessageComponent arg, Dictionary<string, string> info)
        {
            var modal = _modalService.GetModalByName("ManagerDepositModal", info);

            await arg.RespondWithModalAsync(modal.Build());
        }

        public ManagerDepositBtn(ModalService modalservice)
        {
            _modalService = modalservice;
        }
    }
}
