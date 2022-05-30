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

namespace Middleware.Buttons
{
    public class ButtonService : ComponentService<ButtonBuilder, SocketMessageComponent, ButtonBase, ButtonBuilder>
    {

        public override ButtonBuilder GetComponentByName<T>(Dictionary<string, string> adinfo = null)
        {

            var btn = CreateComponent(typeof(T), adinfo, Client, null);
            
            


            var btncomponent = btn.GetComponent();
            btncomponent.CustomId = btn.CustomId;
            return btncomponent;
        }

        public override async Task ExecuteComponentAsync(SocketMessageComponent arg)
        {
            var btn = _addedComponentsTypes.FirstOrDefault(x => x.Name == arg.Data.CustomId.Split("_")[1]);
            if (btn == null) return;

            var infostring = arg.Data.CustomId.Split("_")[2];
            Dictionary<string, string> info = JsonConvert.DeserializeObject<Dictionary<string, string>>(infostring);
            await CreateComponent(btn, info, Client, (arg.User as SocketGuildUser).Guild).OnComponentExecuted(arg);
        }


    }
}
