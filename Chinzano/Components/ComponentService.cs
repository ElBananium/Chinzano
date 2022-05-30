using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Components
{
    public abstract class ComponentService<TBuildInfo, TArgument, TComponent, TBuildedComponent> where TComponent : ComponentBase<TBuildInfo, TArgument>
    {
        protected IServiceProvider _serviceProvider;

        protected List<Type> _addedComponentsTypes = new();

        protected DiscordSocketClient Client;


        protected TComponent CreateComponent(Type componenttype, Dictionary<string, string> adinfo, DiscordSocketClient client, SocketGuild guild)
        {

            List<object> args = new List<object>();

            var constructor = componenttype.GetConstructors().First();
            var parameters = constructor.GetParameters();
            if (parameters.Length != 0)
            {




                foreach (var parameter in parameters)
                {
                    Type paramtype = parameter.ParameterType;

                    args.Add(_serviceProvider.GetRequiredService(paramtype));
                }
            }
            var result = constructor.Invoke(args.ToArray()) as TComponent;
            result.AdditionalInfo = adinfo;
            result.Client = client;
            result.Guild = guild;
            return result;
        }

        public void AddModules(Assembly assembly, IServiceProvider provider, DiscordSocketClient client)
        {
            _serviceProvider = provider;
            Client = client;

            var buttontypes = assembly.GetTypes().Where(x => x.BaseType == typeof(TComponent)).ToArray();

            foreach (var btn in buttontypes)
            {
                

                _addedComponentsTypes.Add(btn);
            }

        }

        public abstract TBuildedComponent GetComponentByName<T>(Dictionary<string, string> adinfo = null) where T : TComponent;

        public abstract Task ExecuteComponentAsync(TArgument arg);




    }
}
