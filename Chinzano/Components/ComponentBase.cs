using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Components
{
    public abstract class ComponentBase<TBuildInfo, TArgument>
    {
        public string CustomId
        {
            get
            {
                return "MiddleWareBotComponent_" + this.GetType().Name + "_" + JsonConvert.SerializeObject(AdditionalInfo);
            }
            
        }

        public Dictionary<string, string> AdditionalInfo { get; set; }

        public DiscordSocketClient Client { get; set; }

        public SocketGuild Guild { get; set; }

        public ComponentBase()
        {
            AdditionalInfo = new Dictionary<string, string>();
        }


        public abstract TBuildInfo GetComponent();

        public abstract Task OnComponentExecuted(TArgument arg);
    }
}
