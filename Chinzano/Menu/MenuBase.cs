using Discord;
using Discord.WebSocket;
using Middleware.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Menu
{
    public abstract class MenuBase : ComponentBase<IEnumerable<SelectMenuOptionBuilder>, SocketMessageComponent>
    {


        public abstract string PlaceHolder { get; }
        public abstract int MinValue { get; }
        public abstract int MaxValue { get; }


    }
}
