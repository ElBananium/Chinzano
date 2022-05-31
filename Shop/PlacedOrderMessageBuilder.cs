using Data.TradeRepository;
using Discord;
using Middleware;
using Middleware.Buttons;
using Shop.Buttons;
using Shop.Services.PlacedOrderRepository;
using Shop.Services.ShopPriceHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public static class PlacedOrderMessageBuilder
    {
        public  static MessageComponent GetComponent(PlacedOrder order)
        {
            var component = new AdditionalComponentBuilder();
            
            
            var dict = new Dictionary<string, string>();
            dict.Add("orderid", order.Id.ToString());


            

            if (!order.IsRecived)
            {

                component.WithButton<RecivePlacedOrderBtn>(dict);
                
            }

            if(order.IsRecived && !order.IsPicked)
            {
                component.WithButton<PickPlacedOrderBtn>(dict);
                
            }
            if (order.IsPicked)
            {
                component.WithButton<OrderIsTransacted>(dict);
                
            }
            component.WithButton<CancelOrder>(dict);
            return component.Build();
        }


        public static EmbedBuilder GetEmbed(PlacedOrder order, IGenericRepository genericRepository, IShopPriceHandler shopPriceHandler)
        {
            var repo = genericRepository.GetRepositoryByName(order.TradeRepoName);
            var embed = new EmbedBuilder() { Title = "Заказ#" + order.Id, Color = Color.Magenta };
            embed.AddField("Что заказано?", repo.PublicName);
            embed.AddField("Сколько заказано?", order.HowManyOrdered);
            embed.AddField("К оплате", shopPriceHandler.GetPrice(repo, order.HowManyOrdered) * order.HowManyOrdered);
            embed.AddField("Когда доставить?", order.WhatTime);

            embed.AddField("Статус товара", order.IsRecived ? "Зарезервирован" : "Не зарезервирован");

            embed.AddField("Принято менеджером:", order.IsPicked ? order.WhosPickedNickname : "Не принятно");

            return embed;
        }
    }
}
