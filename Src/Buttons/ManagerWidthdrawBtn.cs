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

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Снять", Style = ButtonStyle.Danger };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            var modal = _modalService.GetComponentByName("ManagerWidthDrawModal", AdditionalInfo);

            await arg.RespondWithModalAsync(modal.Build());
        }

        public ManagerWidthdrawBtn(IGenericRepository repo, ModalService modalService)
        {
            _repo = repo;

            _modalService = modalService;
        }
    }
}
