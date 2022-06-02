using Discord;
using Discord.WebSocket;
using Middleware;
using Middleware.Menu;
using Shop.Modals;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Menus
{
    public class GetGraphicsOrTableMenu : MenuBase
    {
        private IShopGenericRepository _shopGenericRepository;

        public override string PlaceHolder => "Список";

        public override int MinValue => 1;

        public override int MaxValue => 1;

        public override IEnumerable<SelectMenuOptionBuilder> GetComponent()
        {
            var list = new List<SelectMenuOptionBuilder>();

            list.Add(new("Прибыль", "profit"));
            foreach(var repo in _shopGenericRepository.GetAllRepositories())
            {
                list.Add(new($"Продажи : {repo.PublicName}", repo.Name));
            }
            

            return list;
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            AdditionalInfo.Add("choose", arg.Data.Values.First());
            var compbuilder = AdditionalComponentBuilder.GetModal<DrawGraphicOrTable>(AdditionalInfo);
            await arg.RespondWithModalAsync(compbuilder.Build());

            var msgs = (await arg.Channel.GetMessagesAsync(100).FlattenAsync()).ToArray();
            if (msgs.Length > 2)
            {
                for(int i = 0; i < msgs.Length-2; i++)
                {
                    await msgs[i].DeleteAsync();
                }
            }
        }

        public GetGraphicsOrTableMenu(IShopGenericRepository shopGenericRepository)
        {
            _shopGenericRepository = shopGenericRepository;
        }
    }
}
