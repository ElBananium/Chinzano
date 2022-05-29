using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Buttons;
using Middleware.Menu;
using Shop.Services.OrderSessionRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class OrderShopHandler
    {
        private IGenericRepository _repo;
        private MenuService _menuService;
        private IOrderSessionRepository _ordersessionrepository;
        private DiscordSocketClient _client;
        private ButtonService _btnservice;


        public async Task HandleMessage(SocketMessage arg)
        {
            var ordersession = _ordersessionrepository.GetSession(arg.Channel.Id);
            if (ordersession == null || arg.Author.IsBot) return;

            switch (ordersession.State)
            {
                case OrderSessionState.HowManyNeed:
                    await HandleHowManyNeed(arg, ordersession);
                    break;
                case OrderSessionState.GetCurrentTime:

                    await HandleCurrentTime(arg, ordersession);
                    break;

            }
        }

        public async Task HandleHowManyNeed(SocketMessage arg, OrderSession session)
        {
            uint howmanyneeds = 0;
            if(!uint.TryParse(arg.Content, out howmanyneeds))
            {
                await arg.Channel.SendMessageAsync("Вы ввели некорректное значение");
                return;
            }

            session.State = OrderSessionState.WhatTime;

            session.HowManyNeed = howmanyneeds;

            await (arg.Channel as SocketGuildChannel).AddPermissionOverwriteAsync(arg.Author, new(sendMessages: PermValue.Deny, viewChannel: PermValue.Allow));
            
            var embed = new EmbedBuilder() { Color = Color.Gold, Title = "Ко скольки вам доставить?" };
            embed.AddField("Сейчас", "Если вы хотите получить ваш товар как можно быстрее");
            embed.AddField("Завтра", "Если вы хотите получить ваш товар завтра. Конкретное время вы сможете уточнить позже.");
            embed.AddField("Конкретное время", "Если вы хотите получить ваш товар сегодня, но к конкретному времени");

            var compbuilder = new ComponentBuilder().WithSelectMenu(_menuService.GetMenuByName("WhatTimeOrder"));



            await arg.Channel.SendMessageAsync(embed: embed.Build(), components: compbuilder.Build());

        }
        public async Task HandleCurrentTime(SocketMessage arg, OrderSession session)
        {

            try
            {


                var date = DateTime.ParseExact(arg.Content, "HH:mm", null);


                session.State = OrderSessionState.End;

                session.SpecialTime = date;
                session.IsSpecialTimeDelivery = true;
            }
            catch
            {
                await arg.Channel.SendMessageAsync("Вы некорректно указали время, напишите ещё раз");
                return;
            }
            

            await Recive(session, arg.Channel, arg.Content);





            



            
        }

        private async Task Recive(OrderSession session, ISocketMessageChannel channel, string time)
        {
            var repo = _repo.GetRepositoryByName(session.TradeRepoName);

            var embedbuilder = new EmbedBuilder() { Color = Color.Teal, Title = "Ваш заказ" };
            embedbuilder.AddField($"Вы заказали", repo.PublicName);
            embedbuilder.AddField($"Количество", session.HowManyNeed);
            embedbuilder.AddField("Цена", repo.PricePerItem * session.HowManyNeed);
            embedbuilder.AddField($"Вам доставят сегодня, к ", time);
            
            var compbuilder = new ComponentBuilder().WithButton(_btnservice.GetButtonByName("SubmitOrderBtn", null)).WithButton(_btnservice.GetButtonByName("CloseOrderButton", null));

            await channel.SendMessageAsync(embed: embedbuilder.Build(), components: compbuilder.Build());



        }

        public OrderShopHandler(IOrderSessionRepository repos, MenuService menuService, IGenericRepository repo, DiscordSocketClient client, ButtonService btnService)
        {
            _repo = repo;
            _menuService = menuService;
            _ordersessionrepository = repos;
            _client = client;
            _btnservice = btnService;
        }




    }
}
