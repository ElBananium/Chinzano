using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Menu
{
    public class MenuService
    {
        private IServiceProvider _serviceProvider;

        private Dictionary<string, Type> _menubuilders;



        public MenuService()
        {
            _menubuilders = new Dictionary<string, Type>();
        }

        public MenuService AddServiceProvider(IServiceProvider provider)
        {
            _serviceProvider = provider;
            return this;
        }


        private MenuBase CreateMenu(Type Modal)
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
            return constructor.Invoke(args.ToArray()) as MenuBase;
        }

        public async Task ExecuteMenuAsync(SocketMessageComponent arg, DiscordSocketClient client)
        {
            var btn = _menubuilders[arg.Data.CustomId];
            if (btn == null) return;




            var modalbase = CreateMenu(btn);
            modalbase.Client = client;
            if (arg.Data.Values.Any(x => x == "notchoisen"))
            {
                await arg.DeferAsync();
                return;
            }
            await modalbase.HandleMenu(arg);

        }

        public void AddModules(Assembly assembly)
        {
            var modalstypes = assembly.GetTypes().Where(x => x.BaseType == typeof(MenuBase)).ToArray();

            foreach (var modalbuilder in modalstypes)
            {
                var resbtn = CreateMenu(modalbuilder);

                _menubuilders.Add(resbtn.CustomId, modalbuilder);
            }

        }

        public SelectMenuBuilder GetMenuByName(string name)
        {
            var modalbuilder = _menubuilders.Values.First(x => x.Name == name);

            var modadd = CreateMenu(modalbuilder);


            var modal = new SelectMenuBuilder() { CustomId = modadd.CustomId, Placeholder = modadd.PlaceHolder, MinValues = modadd.MinValue, MaxValues = modadd.MaxValue}.WithOptions(modadd.GetSelectMenuFields().ToList());


            return modal;

        }
    }
}
