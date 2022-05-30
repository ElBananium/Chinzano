using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Middleware.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Menu
{
    public class MenuService : ComponentService<IEnumerable<SelectMenuOptionBuilder>, SocketMessageComponent, MenuBase, SelectMenuBuilder>
    {
        public override async Task ExecuteComponentAsync(SocketMessageComponent arg)
        {
            var btn = _addedComponentsTypes[arg.Data.CustomId.Split("_")[1]];
            if (btn == null) return;


            var infostring = arg.Data.CustomId.Split("_")[2];
            Dictionary<string, string> info = JsonConvert.DeserializeObject<Dictionary<string, string>>(infostring);

            var modalbase = CreateComponent(btn, info, Client, (arg.User as SocketGuildUser).Guild);
            if (arg.Data.Values.Any(x => x == "notchoisen"))
            {
                await arg.DeferAsync();
                return;
            }
            await modalbase.OnComponentExecuted(arg);
        }


        public override SelectMenuBuilder GetComponentByName(string name, Dictionary<string, string> adinfo = null)
        {
            var modalbuilder = _addedComponentsTypes.Values.First(x => x.Name == name);

            var modadd = CreateComponent(modalbuilder, adinfo, Client, null);


            var modal = new SelectMenuBuilder() { CustomId = modadd.CustomId, Placeholder = modadd.PlaceHolder, MinValues = modadd.MinValue, MaxValues = modadd.MaxValue }.WithOptions(modadd.GetComponent().ToList());


            return modal;
        }

    }
}
