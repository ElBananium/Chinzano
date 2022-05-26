﻿using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Middleware.Buttons;
using Middleware.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Menus
{
    public class OwnerMenu : MenuBase
    {
        public override string PlaceHolder => null;

        private IGenericRepository _repo;
        private ButtonService _btnservice;
        private MenuService _menuService;

        public override int MinValue => 1;

        public override int MaxValue => 1;

        public override SelectMenuOptionBuilder[] GetSelectMenuFields()
        {
            var allrepos = _repo.GetAllRepositories();
            var fields = new List<SelectMenuOptionBuilder>();
            fields.Add(new SelectMenuOptionBuilder("-", "notchoisen", isDefault: true));
            foreach(var repository in allrepos)
            {
                fields.Add(new SelectMenuOptionBuilder(repository.PublicName, repository.Name));
            }

            return fields.ToArray();
        }

        public override async Task HandleMenu(SocketMessageComponent modal)
        {
            await modal.DeferAsync();
            var info = modal.Data.Values.First();

                var repository = _repo.GetRepositoryByName(info);

                var embed = new EmbedBuilder() { Color = new Color(187, 27, 78), Title = repository.PublicName };

                embed.AddField("На складе", repository.Count.ToString());
                embed.AddField("Зарезервировано к продаже", repository.ToTradeCount.ToString());

                var components = new ComponentBuilder();
                var adinfo = new Dictionary<string, string>() { { "repname", info } };
                components.WithButton(_btnservice.GetButtonByName("OwnerEditBtn", adinfo));
            components.WithButton(_btnservice.GetButtonByName("DeleteThisMsgBtn", null));


            
            var messages = await modal.Channel.GetMessagesAsync(100).FlattenAsync();
            if(messages.Count() > 1)
            {
                await messages.First().DeleteAsync();
            }
                
            
            await modal.Channel.SendMessageAsync("", false, embed.Build(), components: components.Build());

        }

        public OwnerMenu(IGenericRepository repo, ButtonService btnservice, MenuService menuService)
        {
            _repo = repo;
            _btnservice = btnservice;
            _menuService = menuService;
        }
    }
}