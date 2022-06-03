using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Buttons;
using Shop.Services.OrderSessionRepository;
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
    public class SubmitOrderBtn : ButtonBase
    {
        private IOrderSessionRepository _orderSession;
        private IGenericRepository _genericRepository;
        private IConfiguration _config;
        private IPlacedOrderRepository _placedOrderRepository;
        private IOrderStateLogger _orderStateLogger;
        private IShopPriceHandler _shopPriceHandler;

        public override ButtonBuilder GetComponent()
        {
            return new ButtonBuilder() { Label = "Подтвердить", Style = ButtonStyle.Success };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent arg)
        {

            await arg.DeferAsync();


            var session = _orderSession.GetSession((ulong)arg.ChannelId);

                if (session == null) return;

                var repo = _genericRepository.GetRepositoryByName(session.TradeRepoName);




                if (session.IsNowDelivery)
                {


                    if (repo.Count < session.HowManyNeed)
                    {
                        await arg.Channel.SendMessageAsync("Увы, у нас на складе недостаточно нужного товара, мы не можем принять ваш заказ.");
                        return;
                    }
                    _orderSession.RemoveSession(session.ChannelId);
                    repo.ToTrade(session.HowManyNeed);
                    await NotifyOrder(session, true, arg);
                    return;
                }
                _orderSession.RemoveSession(session.ChannelId);
                bool isResorved = repo.Count >= session.HowManyNeed;
                if (isResorved) repo.ToTrade(session.HowManyNeed);


                await NotifyOrder(session, isResorved, arg);
                


        }

        private async Task NotifyOrder(OrderSession session, bool IsToTrade, SocketMessageComponent channelwithorder)
        {
            //_config["notifyformanagerchannelid"]
            var channel = Guild.GetTextChannel(ulong.Parse(_config["notifyformanagerchannelid"]));

            string timetodelivery = null;
            if (session.IsNowDelivery) timetodelivery = "Сейчас";
            if (session.IsTomorrowDelivery)
            {
                var datetime = DateTime.Now.AddDays(1);
                timetodelivery = datetime.Day.ToString() + "." + datetime.Month.ToString() + "." + datetime.Year.ToString();
            }
            if (session.IsSpecialTimeDelivery) timetodelivery = "Сегодня, к" + session.SpecialTime.Hour + ":" + session.SpecialTime.Minute;

            if (timetodelivery == null) throw new ArgumentNullException();

            var price = _shopPriceHandler.GetPrice(_genericRepository.GetRepositoryByName(session.TradeRepoName), (int)session.HowManyNeed);

            var order = _placedOrderRepository.CreateOrder(session.TradeRepoName, (int)session.HowManyNeed, timetodelivery, IsToTrade, session.ChannelId, price);

            var embed = PlacedOrderMessageBuilder.GetEmbed(order, _genericRepository, _shopPriceHandler);

            var components = PlacedOrderMessageBuilder.GetComponent(order);


            if (IsToTrade) await _orderStateLogger.OrderRecivedFromSystem(order.Id, _genericRepository.GetRepositoryByName(session.TradeRepoName).PublicName.Split("|")[0], (int)session.HowManyNeed);
            await channel.SendMessageAsync(embed: embed.Build(), components: components);

            await channelwithorder.Channel.SendMessageAsync("Скоро вам напишет менеджер по продажам, с ним вы сможете обсудить детали");
            await (channelwithorder.Channel as SocketGuildChannel).AddPermissionOverwriteAsync(channelwithorder.User, new(viewChannel: PermValue.Allow, sendMessages: PermValue.Allow));

            await (channelwithorder.Channel as SocketGuildChannel).ModifyAsync(x => x.Name = "Заказ номер " + order.Id);

            await channelwithorder.Message.ModifyAsync(x => x.Components = new ComponentBuilder().Build() );




        }




        public SubmitOrderBtn(IOrderSessionRepository orderSession, IGenericRepository genericRepository, IConfiguration config, IPlacedOrderRepository placedOrderRepository, IOrderStateLogger orderStateLogger, IShopPriceHandler shopPriceHandler)
        {
            _orderSession = orderSession;
            _genericRepository = genericRepository;
            _config = config;
            _placedOrderRepository = placedOrderRepository;
            _orderStateLogger = orderStateLogger;
            _shopPriceHandler = shopPriceHandler;
        }
    }
}
