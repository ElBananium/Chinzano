using Discord;
using Discord.WebSocket;
using Middleware.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Modals
{
    public abstract class ModalBase : ComponentBase<ModalComponentBuilder, SocketModal>
    {


        public abstract string Title { get; }


        public Dictionary<string, string> TextInputsValues { get; set; }
    }
}
