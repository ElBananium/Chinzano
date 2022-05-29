using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src
{
    public static class AntiExceptionHelper
    {
        public static ulong TechLogChannelId;
        public static ulong CurrentGuildId;

        public static DiscordSocketClient client;

        public static async Task Log(Discord.LogMessage arg)
        {
            if (arg.Exception == null) return;

            var techchannel = client.GetGuild(CurrentGuildId).GetTextChannel(TechLogChannelId);
            await techchannel.SendMessageAsync(arg.Exception.Message);
            await techchannel.SendMessageAsync(arg.Exception.StackTrace);

        }


    }
}
