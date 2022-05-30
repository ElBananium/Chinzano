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

namespace Middleware.Modals
{
    public class ModalService : ComponentService<ModalComponentBuilder, SocketModal, ModalBase, ModalBuilder>
    {
        public override async Task ExecuteComponentAsync(SocketModal arg)
        {
            var btn = _addedComponentsTypes.FirstOrDefault(x =>x.Name == arg.Data.CustomId.Split("_")[1]);
            if (btn == null) return;

            
            var infostring = arg.Data.CustomId.Split("_")[2];
            var adinfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(infostring);

            var modalbase = CreateComponent(btn,adinfo, Client, (arg.User as SocketGuildUser).Guild );
            var dict = new Dictionary<string, string>();

            foreach (var input in arg.Data.Components)
            {
                dict.Add(input.CustomId, input.Value);
            }

            modalbase.TextInputsValues = dict;


            await modalbase.OnComponentExecuted(arg);
        }


        public override ModalBuilder GetComponentByName<T>(Dictionary<string, string> adinfo = null)
        {

            var modadd = CreateComponent(typeof(T), adinfo, Client, null);
            modadd.AdditionalInfo = adinfo;

            var modal = new ModalBuilder() { CustomId = modadd.CustomId, Title = modadd.Title, Components = modadd.GetComponent() };


            return modal;
        }


    }
}
