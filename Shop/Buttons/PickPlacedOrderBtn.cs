using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Buttons;
using Shop.Services.OrderStateLogger;
using Shop.Services.PlacedOrderRepository;
using Shop.Services.ShopPriceHandler;
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
        private IConfiguration _config;
        private IOrderStateLogger _orderStateLogger;
        private IShopPriceHandler _shopPriceHandler;

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Взять заказ", Style = ButtonStyle.Success };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            await arg.DeferAsync();
            int orderid = int.Parse(AdditionalInfo["orderid"]);

            var order = _placedOrderRepository.GetOrder(orderid);

            order.IsPicked = true;
            order.WhosPickedId = arg.User.Id;
            order.WhosPickedNickname = (arg.User as SocketGuildUser).DisplayName.Split("|")[0];



            var embed = PlacedOrderMessageBuilder.GetEmbed(order, _genericRepository, _shopPriceHandler);
            var components = PlacedOrderMessageBuilder.GetComponent(order);

            await _orderStateLogger.OrderPicked(orderid, order.WhosPickedNickname);
            await Guild.GetTextChannel(order.ChannelId).AddPermissionOverwriteAsync(arg.User, new(viewChannel: PermValue.Allow, sendMessages: PermValue.Allow));
            
            await arg.Message.ModifyAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components;

            });




        }

        public PickPlacedOrderBtn(IPlacedOrderRepository placedOrderRepository, IGenericRepository genericRepository, IConfiguration config, IOrderStateLogger orderStateLogger, IShopPriceHandler shopPriceHandler)
        {
            _placedOrderRepository = placedOrderRepository;
            _genericRepository = genericRepository;
            _config = config;
            _orderStateLogger = orderStateLogger;
            _shopPriceHandler = shopPriceHandler;
        }
    }
}
