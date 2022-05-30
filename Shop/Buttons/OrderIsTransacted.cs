﻿using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware;
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
    public class OrderIsTransacted : ButtonBase
    {
        private IPlacedOrderRepository _placedOrderRepository;
        private IConfiguration _config;
        private IGenericRepository _genericRepository;
        private IOrderStateLogger _orderStateLogger;

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Закрыть заказ", Style = ButtonStyle.Danger };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {
            int orderid = int.Parse(AdditionalInfo["orderid"]);

            var order = _placedOrderRepository.GetOrder(orderid);

            if(arg.User.Id != order.WhosPickedId)
            {
                var embed = new EmbedBuilder() { Title = "Ошибка", Color = Color.Red }.AddField("Причина", "Не вы приняли заказ, вы не можете его закрыть");
                await arg.User.SendMessageAsync(embed: embed.Build());
                await arg.DeferAsync();
                return;

            }
            _genericRepository.GetRepositoryByName(order.TradeRepoName).Traded(order.HowManyOrdered);
            
            
            
            var resultembed = new EmbedBuilder() { Title = "Менеджер закрыл ваш заказ", Color = Color.Green }.AddField("Ваш обсуживал менеджер", order.WhosPickedNickname).AddField("Не забудьте оставить отзыв", "Удачи вам");
            
            
            var compbuilder = new AdditionalComponentBuilder().WithButton<ExitOrderButton>().Build();
            await Guild.GetTextChannel(order.ChannelId).AddPermissionOverwriteAsync(arg.User, new(viewChannel: PermValue.Deny));
            await Guild.GetTextChannel(order.ChannelId).SendMessageAsync(embed: resultembed.Build(), components: compbuilder) ;

            var archiveembed = PlacedOrderMessageBuilder.GetEmbed(order, _genericRepository);

            await Guild.GetTextChannel(ulong.Parse(_config["orderarchivechannelid"])).SendMessageAsync(embed: archiveembed.Build());
            await _orderStateLogger.OrderTransacted((arg.User as SocketGuildUser).DisplayName.Split("|")[0], orderid, _genericRepository.GetRepositoryByName(order.TradeRepoName).PublicName);

            _placedOrderRepository.DeleteOrder(orderid);




            await arg.Message.DeleteAsync();
        }

        public OrderIsTransacted(IPlacedOrderRepository placedOrderRepository, IConfiguration config,  IGenericRepository genericRepository, IOrderStateLogger orderStateLogger)
        {
            _placedOrderRepository = placedOrderRepository;
            _config = config;
            _genericRepository = genericRepository;
            _orderStateLogger = orderStateLogger;
        }
    }
}
