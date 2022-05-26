using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.Menu
{
    public abstract class MenuBase
    {
        public string CustomId { get => "ChinzanoBotMenu_" + this.GetType().Name; }

        public abstract string PlaceHolder { get; }
        public abstract int MinValue { get; }
        public abstract int MaxValue { get; }

        public DiscordSocketClient Client { get; set; }

        public abstract SelectMenuOptionBuilder[] GetSelectMenuFields();


        public abstract Task HandleMenu(SocketMessageComponent modal);
    }
}
