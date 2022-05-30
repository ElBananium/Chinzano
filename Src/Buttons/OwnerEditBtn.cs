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
    public class OwnerEditBtn : ButtonBase
    {
        private IGenericRepository _repo;

        private ModalService _modalService;

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Редактировать", Style = ButtonStyle.Primary };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            var modal = _modalService.GetComponentByName("OwnerEditModal", AdditionalInfo);

            await arg.RespondWithModalAsync(modal.Build());
        }
        public OwnerEditBtn(IGenericRepository repo, ModalService modalService )
        {
            _repo = repo;
            _modalService = modalService;
        }
    }
}
