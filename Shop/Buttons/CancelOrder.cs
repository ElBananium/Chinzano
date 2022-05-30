using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Buttons;
using Shop.Services.OrderStateLogger;
using Shop.Services.PlacedOrderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Buttons
{
    public class CancelOrder : ButtonBase
    {
        private IPlacedOrderRepository _placedOrderRepository;
        private DiscordSocketClient _client;
        private IConfiguration _config;
        private ButtonService _buttonService;
        private IGenericRepository _genericRepository;
        private IOrderStateLogger _orderStateLogger;

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Отменить заказ", Style = ButtonStyle.Secondary };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            int orderid = int.Parse(AdditionalInfo["orderid"]);

            var order = _placedOrderRepository.GetOrder(orderid);
            var repo = _genericRepository.GetRepositoryByName(order.TradeRepoName);
            if (order.IsRecived)
            {
                repo.Deposit(order.HowManyOrdered);
                repo.Traded(order.HowManyOrdered);
            }


            var resultembed = new EmbedBuilder() { Title = "Менеджер отменил ваш заказ", Color = Color.Red }.AddField("Заказ отменил менеджер :", (arg.User as SocketGuildUser).DisplayName.Split("|")[0]);


            var compbuilder = new ComponentBuilder().WithButton(_buttonService.GetComponentByName("ExitOrderButton", null)).Build();
            await _client.GetGuild(ulong.Parse(_config["currentguildid"])).GetTextChannel(order.ChannelId).AddPermissionOverwriteAsync(arg.User, new(viewChannel: PermValue.Deny));
            await _client.GetGuild(ulong.Parse(_config["currentguildid"])).GetTextChannel(order.ChannelId).SendMessageAsync(embed: resultembed.Build(), components: compbuilder);

            var archiveembed = PlacedOrderMessageBuilder.GetEmbed(order, _genericRepository);

            await _client.GetGuild(ulong.Parse(_config["currentguildid"])).GetTextChannel(ulong.Parse(_config["orderarchivechannelid"])).SendMessageAsync(embed: archiveembed.Build());
            await _orderStateLogger.OrderCanceled((arg.User as SocketGuildUser).DisplayName.Split("|")[0], orderid, repo.PublicName);

            _placedOrderRepository.DeleteOrder(orderid);




            await arg.Message.DeleteAsync();
        }

        public CancelOrder(IPlacedOrderRepository placedOrderRepository, DiscordSocketClient client, IConfiguration config, ButtonService buttonService, IGenericRepository genericRepository, IOrderStateLogger orderStateLogger)
        {
            _placedOrderRepository = placedOrderRepository;
            _client = client;
            _config = config;
            _buttonService = buttonService;
            _genericRepository = genericRepository;
            _orderStateLogger = orderStateLogger;
        }
    }
}
