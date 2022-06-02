using Discord;
using Discord.WebSocket;
using GraphDrawing.GraphDrawer;
using GraphDrawing.GraphImageService;
using Middleware;
using Middleware.Modals;
using Shop.Buttons;
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

        public override string Title => "Укажите данные";

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
            await arg.DeferAsync();
            var info = new List<SalesGraphPoint>();

                if (AdditionalInfo["choose"] == "profit") info = _graphicsOrTableInfoHandler.GetProfitInfo((int)mount, (int)from, (int)to).ToList();

                else
                {
                    info = _graphicsOrTableInfoHandler.GetSalesInfo(AdditionalInfo["choose"], (int)mount, (int)from, (int)to).ToList();
                }


                if (AdditionalInfo["info"] == "graph") await SendGraphicImage(info, arg);
            if (AdditionalInfo["info"] == "table") await SendTable(info, arg);



            

        }

        private async Task SendGraphicImage(IEnumerable<SalesGraphPoint> point, SocketModal modal)
        {
            var img = _imgService.GetFilePatch(point);

            var components = new AdditionalComponentBuilder().WithButton<DeleteThisMsgBtn>();

            await modal.Channel.SendFileAsync(img, components: components.Build());

            File.Delete(img);
        }
        private async Task SendTable(IEnumerable<SalesGraphPoint> points, SocketModal modal)
        {
            var embed = new EmbedBuilder() { Title = "Таблица", Color = Color.Blue };

            foreach(var point in points)
            {
                embed.AddField("Число : "+point.Day.ToString(), "Показатель : "+point.Count);
            }
            var components = new AdditionalComponentBuilder().WithButton<DeleteThisMsgBtn>();

            await modal.Channel.SendMessageAsync(embed: embed.Build(), components: components.Build());

        }


        public DrawGraphicOrTable(IGraphicsOrTableInfoHandler graphicsOrTableInfoHandler, IGraphImageService imgservice)
        {
            _graphicsOrTableInfoHandler = graphicsOrTableInfoHandler;
            _imgService = imgservice;
        }
    }
}
