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
    public class RegistrationBtn : ButtonBase
    {

        private ModalService _modalService;
        public override ButtonBuilder GetButton()
        {
            return new ButtonBuilder() { Label = "Сменить никнейм", Style = ButtonStyle.Primary };
        }

        public async override Task OnButtonClicked(SocketMessageComponent arg, Dictionary<string,string> info)
        {
            var modal = _modalService.GetModalByName("RegistrationModal", null);
            await arg.RespondWithModalAsync(modal.Build());
        }

        public RegistrationBtn(ModalService modalservice)
        {
            _modalService = modalservice;
        }
    }
}
