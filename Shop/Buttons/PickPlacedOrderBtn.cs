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
    public class PickPlacedOrderBtn : ButtonBase
    {
        private IPlacedOrderRepository _placedOrderRepository;
        private IGenericRepository _genericRepository;
        private ButtonService _buttonService;
        private DiscordSocketClient _client;
        private IConfiguration _config;
        private IOrderStateLogger _orderStateLogger;

        public override ButtonBuilder GetButton()
        {
            return new ButtonBuilder() { Label = "Взять заказ", Style = ButtonStyle.Success };
        }

        public override async Task OnButtonClicked(SocketMessageComponent arg, Dictionary<string, string> info)
        {
            int orderid = int.Parse(info["orderid"]);

            var order = _placedOrderRepository.GetOrder(orderid);

            order.IsPicked = true;
            order.WhosPickedId = arg.User.Id;
            order.WhosPickedNickname = (arg.User as SocketGuildUser).DisplayName.Split("|")[0];



            var embed = PlacedOrderMessageBuilder.GetEmbed(order, _genericRepository);
            var components = PlacedOrderMessageBuilder.GetComponent(order, _buttonService);

            await _orderStateLogger.OrderPicked(orderid, order.WhosPickedNickname);
            await _client.GetGuild(ulong.Parse(_config["currentguildid"])).GetTextChannel(order.ChannelId).AddPermissionOverwriteAsync(arg.User, new(viewChannel: PermValue.Allow, sendMessages: PermValue.Allow));
            await arg.DeferAsync();
            await arg.Message.ModifyAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components.Build();

            });




        }

        public PickPlacedOrderBtn(IPlacedOrderRepository placedOrderRepository, IGenericRepository genericRepository, ButtonService buttonService, DiscordSocketClient client, IConfiguration config, IOrderStateLogger orderStateLogger)
        {
            _placedOrderRepository = placedOrderRepository;
            _genericRepository = genericRepository;
            _buttonService = buttonService;
            _client = client;
            _config = config;
            _orderStateLogger = orderStateLogger;
        }
    }
}
