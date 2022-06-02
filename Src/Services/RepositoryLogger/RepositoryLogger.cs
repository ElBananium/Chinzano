using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Services.RepositoryLogger
{
    public class RepositoryLogger : IRepositoryLogger
    {
        private IConfiguration _configuration;

        private DiscordSocketClient _client;

        public async Task LogDeposit(SocketUser user, TradeRepo repo, long count)
        {
            var embed = new EmbedBuilder() { Color = Color.Green };

            string name = _client
                .GetGuild(ulong.Parse(_configuration["currentguildid"]))
                .GetUser(user.Id)
                .DisplayName
                .Split(" | ")[0];

            embed.Title = $"{repo.PublicName} | {name} положил {count}";

            await SendMessageToPublicLog(embed);

        }

        public async Task LogWidthDraw(SocketUser user, TradeRepo repo, long count)
        {
            var embed = new EmbedBuilder() { Color = Color.Red };

            string name = _client
                .GetGuild(ulong.Parse(_configuration["currentguildid"]))
                .GetUser(user.Id)
                .DisplayName
                .Split(" | ")[0];

            embed.Title = $"{repo.PublicName} | {name} снял  {count}";

            await SendMessageToPublicLog(embed);

        }

        public async Task SendMessageToPublicLog(EmbedBuilder embed)
        {
            await _client.GetGuild(ulong.Parse(_configuration["currentguildid"]))
                .GetTextChannel(ulong.Parse(_configuration["publiclogchannelid"]))
                .SendMessageAsync(embed: embed.Build());
        }


        public async Task LogCraft(SocketUser user, TradeRepo fromrepo, TradeRepo toRepo, long howmanyused, long howmanycrafted)
        {
                var embed = new EmbedBuilder() { Color = Color.Blue };

                string name = _client
                    .GetGuild(ulong.Parse(_configuration["currentguildid"]))
                    .GetUser(user.Id)
                    .DisplayName
                    .Split(" | ")[0];

                embed.Title = $"Крафт | {name} |{fromrepo.PublicName} ({howmanyused})->{toRepo.PublicName}({howmanycrafted})";

                await SendMessageToPublicLog(embed);

        }

        public RepositoryLogger(IConfiguration config, DiscordSocketClient client)
        {
            _configuration = config;

            _client = client;
        }
    }
}
