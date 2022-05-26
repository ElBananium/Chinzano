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

namespace Middleware.Buttons
{
    public class ButtonService
    {
        private IServiceProvider _serviceProvider;

        private Dictionary<string, Type> _buttons;

        public ButtonService()
        {
            _buttons = new Dictionary<string, Type>();
        }

        public ButtonService AddServiceProvider(IServiceProvider provider)
        {
            _serviceProvider = provider;
            return this;
        }

        private ButtonBase CreateButton(Type button)
        {

                List<object> args = new List<object>();

                var constructor = button.GetConstructors().First();
                var parameters = constructor.GetParameters();
                if (parameters.Length != 0)
                {




                    foreach (var parameter in parameters)
                    {
                        Type paramtype = parameter.ParameterType;

                        args.Add(_serviceProvider.GetRequiredService(paramtype));
                    }
                }
                return constructor.Invoke(args.ToArray()) as ButtonBase;
        }
        public async Task ExecuteButtonAsync(SocketMessageComponent arg)
        {
            var btn = _buttons[arg.Data.CustomId.Split("_")[1]];
            if (btn == null) return;

            var infostring = arg.Data.CustomId.Split("_")[2];
            Dictionary<string, string> info = JsonConvert.DeserializeObject<Dictionary<string, string>>(infostring);
            await CreateButton(btn).OnButtonClicked(arg, info);

        }

        public void AddModules(Assembly assembly)
        {
            var buttontypes = assembly.GetTypes().Where(x => x.BaseType == typeof(ButtonBase)).ToArray();

            foreach(var btn in buttontypes)
            {
                var resbtn = CreateButton(btn);

                _buttons.Add(resbtn.CustomId.Split("_")[1], btn);
            }
            
        }

        public ButtonBuilder GetButtonByName(string name, Dictionary<string,string> adinfo)
        {
            var btntype = _buttons.Values.First(x => x.Name == name);

            var btn = CreateButton(btntype);
            btn.AdditionalInfo = adinfo;
            var btncomponent = btn.GetButton();

            btncomponent.CustomId = btn.CustomId;

            return btncomponent;

        }

    }
}
