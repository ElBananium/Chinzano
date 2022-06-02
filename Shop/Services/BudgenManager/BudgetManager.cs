using Data.MoneyStorage;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Shop.Services.PlacedOrderRepository;
using Shop.Services.ShopPriceHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.BudgenManager
{
    public class BudgetManager : IBudgetManager
    {
        private IMoneyStorage _moneyStorage;
        private IShopGenericRepository _repo;
        private IShopPriceHandler _shopPriceHandler;
        private ulong _moneylogid;
        private ulong _guildid;
        private DiscordSocketClient _client;

        public async Task OrderMadeProfit(PlacedOrder order)
        {
            var repo = _repo.GetRepositoryByName(order.TradeRepoName);

            var result = _shopPriceHandler.GetPrice(repo, order.HowManyOrdered) * order.HowManyOrdered;
            _moneyStorage.Deposit((uint)result);

            var embed = new EmbedBuilder() { Color = Color.Green, Title = $"Заказ #{order.Id} закрыт | Прибыль : {result} | Бюджет : {_moneyStorage.Count}" };

            await _client.GetGuild(_guildid).GetTextChannel(_moneylogid).SendMessageAsync(embed: embed.Build());
        }

        public async Task ManagerWidthDrawSomeMoney(string managername, int count, string reason)
        {


            _moneyStorage.WidthDraw((uint)count);

            var embed = new EmbedBuilder() { Color = Color.Red, Title = $"Менеджер : {managername} снял деньги | Бюджет : {_moneyStorage.Count}" };

            embed.AddField("Причина", reason);
            embed.AddField("Количество", count);

            await _client.GetGuild(_guildid).GetTextChannel(_moneylogid).SendMessageAsync(embed: embed.Build());


        }

        public BudgetManager(IMoneyStorage moneyStorage,IShopGenericRepository repo, IShopPriceHandler shopPriceHandler, DiscordSocketClient client ,IConfiguration config)
        {
            _moneyStorage = moneyStorage;
            _repo = repo;
            _shopPriceHandler = shopPriceHandler;
            _moneylogid = ulong.Parse(config["moneylogchannelid"]);
            _guildid = ulong.Parse(config["currentguildid"]);
            _client = client;
        }
    }
}
