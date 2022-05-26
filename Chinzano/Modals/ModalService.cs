using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Modals
{
    public class ModalService
    {

        private IServiceProvider _serviceProvider;

        private Dictionary<string, Type> _modalsbuilders;



        public ModalService()
        {
            _modalsbuilders = new Dictionary<string, Type>();
        }

        public ModalService AddServiceProvider(IServiceProvider provider)
        {
            _serviceProvider = provider;
            return this;
        }


        private ModalBase CreateModal(Type Modal)
        {

            List<object> args = new List<object>();

            var constructor = Modal.GetConstructors().First();
            var parameters = constructor.GetParameters();
            if (parameters.Length != 0)
            {




                foreach (var parameter in parameters)
                {
                    Type paramtype = parameter.ParameterType;

                    args.Add(_serviceProvider.GetRequiredService(paramtype));
                }
            }
            return constructor.Invoke(args.ToArray()) as ModalBase;
        }

        public async Task ExecuteModalAsync(SocketModal arg, DiscordSocketClient client)
        {
            var btn = _modalsbuilders[arg.Data.CustomId.Split("_")[1]];
            if (btn == null) return;

            var dict = new Dictionary<string, string>();

            foreach(var input in arg.Data.Components)
            {
                dict.Add(input.CustomId, input.Value);
            }

            var modalbase =  CreateModal(btn);
            modalbase.Client = client;
            var infostring = arg.Data.CustomId.Split("_")[2];
            modalbase.AdditionalInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(infostring);

            await modalbase.HandleModal(dict,arg);

        }

        public void AddModules(Assembly assembly)
        {
            var modalstypes = assembly.GetTypes().Where(x => x.BaseType == typeof(ModalBase)).ToArray();

            foreach (var modalbuilder in modalstypes)
            {
                var resbtn = CreateModal(modalbuilder);

                _modalsbuilders.Add(resbtn.CustomId.Split("_")[1], modalbuilder);
            }

        }

        public ModalBuilder GetModalByName(string name, Dictionary<string, string> adinfo)
        {
            var modalbuilder = _modalsbuilders.Values.First(x => x.Name == name);

            var modadd = CreateModal(modalbuilder);
            modadd.AdditionalInfo = adinfo;

            var modal = new ModalBuilder() { CustomId = modadd.CustomId, Title = modadd.Title, Components =  modadd.GetModalsComponent()};


            return modal;

        }

    }
}
