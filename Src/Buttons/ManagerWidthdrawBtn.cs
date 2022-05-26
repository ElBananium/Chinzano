using Data.TradeRepository;
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
    public class ManagerWidthdrawBtn : ButtonBase
    {
        private IGenericRepository _repo;

        private ModalService _modalService;

        public override ButtonBuilder GetButton()
        {
            return new ButtonBuilder() { Label = "Снять", Style = ButtonStyle.Danger };
        }

        public override async Task OnButtonClicked(SocketMessageComponent arg, Dictionary<string, string> info)
        {
            var modal = _modalService.GetModalByName("ManagerWidthDrawModal", info);

            await arg.RespondWithModalAsync(modal.Build());
        }

        public ManagerWidthdrawBtn(IGenericRepository repo, ModalService modalService)
        {
            _repo = repo;

            _modalService = modalService;
        }
    }
}
