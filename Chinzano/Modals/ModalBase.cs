using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Modals
{
    public abstract class ModalBase
    {
        public ModalBase()
        {
            AdditionalInfo = new Dictionary<string, string>();
        }
        public string CustomId { get => "ChinzanoBotModal_" + this.GetType().Name+"_"+ JsonConvert.SerializeObject(AdditionalInfo); }

        public abstract string Title { get; }

        public DiscordSocketClient Client { get; set; }

        public abstract ModalComponentBuilder GetModalsComponent();

        public Dictionary<string, string> AdditionalInfo { get; set; }


        public abstract Task HandleModal(Dictionary<string,string> TextInputsValues, SocketModal modal);

    }
}
