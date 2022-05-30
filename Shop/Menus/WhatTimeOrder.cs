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

namespace Shop.Menus
{
    public class WhatTimeOrder : MenuBase
    {
        private IOrderSessionRepository _orderSessionRepo;
        private IGenericRepository _genericRepository;
        private DiscordSocketClient _client;
        private IConfiguration _config;
        private ButtonService _btnservice;

        public override string PlaceHolder => "Выберите";

        public override int MinValue => 1;

        public override int MaxValue => 1;

        public override SelectMenuOptionBuilder[] GetComponent()
        {
            return new SelectMenuOptionBuilder[] { new SelectMenuOptionBuilder("-","notchoisen", isDefault:true), new SelectMenuOptionBuilder("Сейчас", "now",isDefault: false),
            new SelectMenuOptionBuilder("Завтра", "tomorrow",isDefault: false),
            new SelectMenuOptionBuilder("Конкретное время", "currenttime",isDefault: false) };
        }

        public override async Task OnComponentExecuted(SocketMessageComponent modal)
        {
            
                var session = _orderSessionRepo.GetSession((ulong)modal.ChannelId);
                if (session == null) return;
                if(session.State != OrderSessionState.WhatTime) return;
                await modal.DeferAsync();
                switch (modal.Data.Values.First())
                {
                    case "now":
                        session.IsNowDelivery = true;
                        session.State = OrderSessionState.End;
                        await Recive(session, modal.Channel);
                        break;
                    case "tomorrow":
                        session.IsTomorrowDelivery = true;
                        session.State = OrderSessionState.End;
                        await Recive(session, modal.Channel);
                        break;
                    case "currenttime":
                        session.State = OrderSessionState.GetCurrentTime;
                        session.IsSpecialTimeDelivery = true;
                    await ToCurrentTime(session, modal.Channel, modal.User);
                        break;

                }      


        }

        private async Task Recive(OrderSession session, ISocketMessageChannel channel)
        {
            var repo = _genericRepository.GetRepositoryByName(session.TradeRepoName);
            var embedbuilder = new EmbedBuilder() { Color = Color.Teal, Title = "Ваш заказ"};
            embedbuilder.AddField($"Вы заказали", repo.PublicName);
            embedbuilder.AddField($"Количество", session.HowManyNeed);
            embedbuilder.AddField("Цена", repo.PricePerItem * session.HowManyNeed);
            if (session.IsTomorrowDelivery) embedbuilder.AddField($"Ко времени", "Завтра");

            if (session.IsNowDelivery) embedbuilder.AddField($"Ко времени", "Сейчас");

            var compbuilder = new ComponentBuilder().WithButton(_btnservice.GetComponentByName("SubmitOrderBtn", null)).WithButton(_btnservice.GetComponentByName("CloseOrderButton", null));

            await _client.GetGuild(ulong.Parse(_config["currentguildid"])).GetTextChannel(channel.Id).SendMessageAsync(embed: embedbuilder.Build(), components: compbuilder.Build());
            


        }

        private async Task ToCurrentTime(OrderSession session, ISocketMessageChannel channel, SocketUser user)
        {
            await (channel as SocketGuildChannel).AddPermissionOverwriteAsync(user, new(sendMessages: PermValue.Allow, viewChannel: PermValue.Allow));

            await channel.SendMessageAsync("К какому времени? Укажите в формате : 14:00, по московскому времени");
        }

        public WhatTimeOrder(IOrderSessionRepository orderrepo, IGenericRepository genericRepository, DiscordSocketClient client, IConfiguration config, ButtonService btnservice)
        {
            _orderSessionRepo = orderrepo;
            _genericRepository = genericRepository;
            _client = client;
            _config = config;
            _btnservice = btnservice;
        }
    }
}
