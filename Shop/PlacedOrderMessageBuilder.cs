using Data.TradeRepository;
using Discord;
using Middleware.Buttons;
using Shop.Services.PlacedOrderRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public static class PlacedOrderMessageBuilder
    {
        public  static ComponentBuilder GetComponent(PlacedOrder order, ButtonService buttonService)
        {
            var component = new ComponentBuilder();
            var dict = new Dictionary<string, string>();
            dict.Add("orderid", order.Id.ToString());
            ButtonBuilder btn = null;
            component.WithButton(buttonService.GetComponentByName("CancelOrder", dict));

            if (!order.IsRecived)
            {
                
                btn = buttonService.GetComponentByName("RecivePlacedOrderBtn", dict);
                
            }

            if(order.IsRecived && !order.IsPicked)
            {
                btn = buttonService.GetComponentByName("PickPlacedOrderBtn", dict);
                
            }
            if (order.IsPicked)
            {
                btn = buttonService.GetComponentByName("OrderIsTransacted", dict);
                
            }
            if(btn != null) component.WithButton(btn);

            return component;
        }


        public static EmbedBuilder GetEmbed(PlacedOrder order, IGenericRepository genericRepository)
        {
            var repo = genericRepository.GetRepositoryByName(order.TradeRepoName);
            var embed = new EmbedBuilder() { Title = "Заказ#" + order.Id, Color = Color.Magenta };
            embed.AddField("Что заказано?", repo.PublicName);
            embed.AddField("Сколько заказано?", order.HowManyOrdered);
            embed.AddField("К оплате", order.HowManyOrdered * repo.PricePerItem);
            embed.AddField("Когда доставить?", order.WhatTime);

            embed.AddField("Статус товара", order.IsRecived ? "Зарезервирован" : "Не зарезервирован");

            embed.AddField("Принято менеджером:", order.IsPicked ? order.WhosPickedNickname : "Не принятно");

            return embed;
        }
    }
}
