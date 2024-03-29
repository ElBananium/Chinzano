﻿using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
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
    public class RecivePlacedOrderBtn : ButtonBase
    {
        private IPlacedOrderRepository _placedOrderRepository;
        private IGenericRepository _genericRepository;
        private IOrderStateLogger _orderStateLogger;
        private IShopPriceHandler _shopPriceHandler;

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Зарезервировать товар", Style = ButtonStyle.Primary };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {

            await arg.DeferAsync();

            int orderid = int.Parse(AdditionalInfo["orderid"]);





            var placedorder = _placedOrderRepository.GetOrder(orderid);


            var trrepo = _genericRepository.GetRepositoryByName(placedorder.TradeRepoName);

            if (trrepo.Count < placedorder.HowManyOrdered)
            {
                var errembed = new EmbedBuilder() { Title = "Ошибка", Color = Color.Red };
                errembed.AddField("Причина", "На складе недостаточно товара.");
                await arg.User.SendMessageAsync(embed: errembed.Build());
                await arg.DeferAsync();
                return;
            }

            trrepo.ToTrade(placedorder.HowManyOrdered);


            placedorder.IsRecived = true;

            await _orderStateLogger.OrderRecivedFromManager((arg.User as SocketGuildUser).DisplayName, placedorder.Id, trrepo.PublicName, placedorder.HowManyOrdered);

            var embed = PlacedOrderMessageBuilder.GetEmbed(placedorder, _genericRepository, _shopPriceHandler);
            var components = PlacedOrderMessageBuilder.GetComponent(placedorder);
            
            await arg.Message.ModifyAsync(x =>
            {
                x.Embed = embed.Build();
                x.Components = components;

            });





        }

        public RecivePlacedOrderBtn(IPlacedOrderRepository placedOrderRepository, IGenericRepository genericRepository, IOrderStateLogger orderStateLogger, IShopPriceHandler shopPriceHandler)
        {
            _placedOrderRepository = placedOrderRepository;
            _genericRepository = genericRepository;
            _orderStateLogger = orderStateLogger;
            _shopPriceHandler = shopPriceHandler;
        }
    }
}
