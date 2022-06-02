using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src
{
    public static class AntiExceptionHelper
    {

        public static IConfiguration config;

        public static DiscordSocketClient client;

        public static async Task Log(Discord.LogMessage arg)
        {
            
            if (arg.Exception == null) return;
            ulong currentguildid = ulong.Parse(config["currentguildid"]);
            ulong techlogchannelid = ulong.Parse(config["techlogid"]);
            var techchannel = client.GetGuild(currentguildid).GetTextChannel(techlogchannelid);
            if (arg.Exception.Message != null) await techchannel.SendMessageAsync(arg.Exception.Message);

            if(arg.Exception.StackTrace != null) await techchannel.SendMessageAsync(arg.Exception.StackTrace);

        }

        public static async void LogException(Task arg)
        {
            if (arg.Exception == null) return;
            ulong currentguildid = ulong.Parse(config["currentguildid"]);
            ulong techlogchannelid = ulong.Parse(config["techlogid"]);
            var techchannel = client.GetGuild(currentguildid).GetTextChannel(techlogchannelid);
            if (arg.Exception.Message != null) await techchannel.SendMessageAsync(arg.Exception.Message);
            if(arg.Exception.InnerException != null) await techchannel.SendMessageAsync(arg.Exception.InnerException.ToString());
            if (arg.Exception.StackTrace != null) await techchannel.SendMessageAsync(arg.Exception.StackTrace);
        } 

    }
}
