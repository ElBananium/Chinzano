using Discord;
using Discord.WebSocket;
using GraphDrawing.GraphDrawer;
using GraphDrawing.GraphImageService;
using Middleware.Modals;
using Shop.Services.GraphicsOrTableInfoHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Modals
{
    public class DrawGraphicOrTable : ModalBase
    {
        private IGraphicsOrTableInfoHandler _graphicsOrTableInfoHandler;
        private IGraphImageService _imgService;

        public override string Title => throw new NotImplementedException();

        public override ModalComponentBuilder GetComponent()
        {
            var list = new ModalComponentBuilder();
            list.WithTextInput(new("Введите месяц", "mounth"));

            list.WithTextInput(new("С какого числа", "from"));

            list.WithTextInput(new("До какого числа", "to"));

            return list;    

            
        }

        public override async Task OnComponentExecuted(SocketModal arg)
        {
            uint mount;
            uint from;
            uint to;
            if (!uint.TryParse(TextInputsValues["mounth"], out mount)) return;
            if (!uint.TryParse(TextInputsValues["from"], out from)) return;
            if (!uint.TryParse(TextInputsValues["to"], out to)) return;

            var info = new List<SalesGraphPoint>();

            if (AdditionalInfo["choose"] == "profit") info = _graphicsOrTableInfoHandler.GetProfitInfo((int)mount, (int)from, (int)to).ToList();

            else
            {
                info = _graphicsOrTableInfoHandler.GetSalesInfo(AdditionalInfo["choose"], (int)mount, (int)from, (int)to).ToList();
            }


            if (AdditionalInfo["info"] == "graph") await SendGraphicImage(info, arg);



            await arg.DeferAsync();
        }

        private async Task SendGraphicImage(IEnumerable<SalesGraphPoint> point, SocketModal modal)
        {
            var img = _imgService.GetFilePatch(point);

            await modal.Channel.SendFileAsync(img);
        }


        public DrawGraphicOrTable(IGraphicsOrTableInfoHandler graphicsOrTableInfoHandler, IGraphImageService imgservice)
        {
            _graphicsOrTableInfoHandler = graphicsOrTableInfoHandler;
            _imgService = imgservice;
        }
    }
}
