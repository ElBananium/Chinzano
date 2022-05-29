using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.OrderStateLogger
{
    public class OrderStateLogger : IOrderStateLogger
    {
        private DiscordSocketClient _client;
        private IConfiguration _config;

        public async Task OrderRecivedFromManager(string managername, int numberoforder, string reponame, int count)
        {
            var embed = new EmbedBuilder() { Title = $"{reponame} | Заказ#{numberoforder} | {managername} зарезервировал {count}" };
            await SendMessage(embed);
        }

        public async Task OrderRecivedFromSystem(int numberoforder, string reponame, int count)
        {
            var embed = new EmbedBuilder() { Title = $"{reponame} | Заказ#{numberoforder} | Автоматически зарезервировано {count}", Color = Color.DarkOrange };
            await SendMessage(embed);
        }

        public async Task OrderTransacted(string managername, int numberoforder, string reponame)
        {
            var embed = new EmbedBuilder() { Title = $"{reponame} | Заказ#{numberoforder} | Закрыт", Color = Color.Gold };
            await SendMessage(embed);
        }

        public async Task OrderPicked(int numberoforder, string managername)
        {
            var embed = new EmbedBuilder() { Title = $"{managername} взял заказ#{numberoforder}", Color = Color.LightOrange };
            await SendMessage(embed);
        }

        private async Task SendMessage(EmbedBuilder embed)
        {
            await _client.GetGuild(ulong.Parse(_config["currentguildid"])).GetTextChannel(ulong.Parse(_config["publiclogchannelid"])).SendMessageAsync(embed: embed.Build());
        }

        public OrderStateLogger(DiscordSocketClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }
    }
}
