using Data.ShopPriceFiltersRepository;
using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Middleware.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Buttons
{
    public class UpdatePriceListBtn : ButtonBase
    {
        private IGenericRepository _repos;
        private IShopPriceFiltersRepository _filtersrepos;

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Обновить прайс-лист", Style = ButtonStyle.Primary };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            await arg.DeferAsync();
            
            ulong pricelistchannelid;
            using(StreamReader sr = new StreamReader("pricelistchannelid.txt"))
            {
                pricelistchannelid = ulong.Parse(sr.ReadToEnd());
            }

            var channel = Guild.GetTextChannel(pricelistchannelid);

            var msgs =await channel.GetMessagesAsync(100).FlattenAsync();
            foreach(var msg in msgs)
            {
                await msg.DeleteAsync();
            }
            var embeds = PriceListEmbedsBuilder.GetEmbeds(_filtersrepos, _repos.GetAllRepositories());

            foreach (var embed in embeds)
            {
                await channel.SendMessageAsync(embed: embed.Build());
            }
        }

        public UpdatePriceListBtn(IGenericRepository repos, IShopPriceFiltersRepository filtersrepos)
        {
            _repos = repos;
            _filtersrepos = filtersrepos;

        }
    }
}
