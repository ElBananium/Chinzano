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

        public override ButtonBuilder GetComponentByName(string name, Dictionary<string, string> adinfo = null)
        {
            var btntype = _addedComponentsTypes.Values.First(x => x.Name == name);

            var btn = CreateComponent(btntype, adinfo, Client, null);
            
            


            var btncomponent = btn.GetComponent();
            btncomponent.CustomId = btn.CustomId;
            return btncomponent;
        }

        public override async Task ExecuteComponentAsync(SocketMessageComponent arg)
        {
            var btn = _addedComponentsTypes[arg.Data.CustomId.Split("_")[1]];
            if (btn == null) return;

            var infostring = arg.Data.CustomId.Split("_")[2];
            Dictionary<string, string> info = JsonConvert.DeserializeObject<Dictionary<string, string>>(infostring);
            await CreateComponent(btn, info, Client, (arg.User as SocketGuildUser).Guild).OnComponentExecuted(arg);
        }


    }
}
