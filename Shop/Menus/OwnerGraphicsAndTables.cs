using Discord;
using Discord.WebSocket;
using Middleware;
using Middleware.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Menus
{
    public class OwnerGraphicsAndTables : MenuBase
    {
        public override string PlaceHolder => "Вы хотите получить график или таблицу?";

        public override int MinValue => 1;

        public override int MaxValue => 1;

        public override IEnumerable<SelectMenuOptionBuilder> GetComponent()
        {
            var list = new List<SelectMenuOptionBuilder>();

            list.Add(new SelectMenuOptionBuilder("График", "graph"));
            list.Add(new SelectMenuOptionBuilder("Таблица", "table"));
            return list;
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            await arg.DeferAsync();

            var msgs = (await arg.Channel.GetMessagesAsync(100).FlattenAsync()).ToArray();
            if (msgs.Length > 1)
            {
                for (int i = 0; i < msgs.Length-1; i++)
                {
                    await msgs[i].DeleteAsync();
                }
            }
            var dict = new Dictionary<string, string>();
            dict.Add("info", arg.Data.Values.First());
            var compbuilder = new AdditionalComponentBuilder().WithSelectMenu<GetGraphicsOrTableMenu>(dict);
           await arg.Channel.SendMessageAsync("Информация о чем вам нужна?", components: compbuilder.Build());

            
            
        }
    }
}
