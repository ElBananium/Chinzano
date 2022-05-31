using Discord;
using Discord.WebSocket;
using Middleware;
using Middleware.Menu;
using Shop.Buttons;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Menus
{
    public class PriceShopRepoMenu : MenuBase
    {
        private IShopGenericRepository _shopgenrepo;

        public override string PlaceHolder => "Редактировать цены";

        public override int MinValue => 1;

        public override int MaxValue => 1;

        public override IEnumerable<SelectMenuOptionBuilder> GetComponent()
        {
            var result = new List<SelectMenuOptionBuilder>();
            foreach(var shoprepo in _shopgenrepo.GetAllRepositories())
            {
                result.Add(new(shoprepo.PublicName, shoprepo.Name));
            }
            return result;
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            var value = arg.Data.Values.First();

            var info = new Dictionary<string, string>();
            info.Add("reponame", value);
            var components = new AdditionalComponentBuilder().WithButton<AddPriceFilterBtn>(info).WithButton<DeletePriceFilterBtn>(info).WithButton<UpdatePriceListBtn>().WithButton<DeleteThisMsgBtn>();

            var msgs = await arg.Channel.GetMessagesAsync(10).FlattenAsync();
            if (msgs.Count() > 1)
            {
                foreach(var msg in msgs.Reverse().Skip(1))
                {
                    await msg.DeleteAsync();
                }
                
            }

            await arg.Channel.SendMessageAsync("Редактировать", components: components.Build());

            await arg.DeferAsync();
        }

        public PriceShopRepoMenu(IShopGenericRepository shoprepo)
        {
            _shopgenrepo = shoprepo;
        }
    }
}
