using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Buttons;
using Middleware.Menu;
using Middleware.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middleware
{
    public class DiscordBotBuilder
    {
        private string _token;
        private bool _useButtonBuilder = false;
        private bool _useCommandHandler = false;
        private bool _useModalHandler = false;
        private bool _useMenuHandler = false;
        private bool _useLogger = false;

        private DiscordBot _bot;
        public DiscordSocketClient Client => _bot.Client;


        public DiscordBotBuilder(string token, DiscordSocketConfig botconfig, IConfiguration configuration)
        {
            _token = token;
            _bot = new DiscordBot(botconfig);
            _bot.Configuration = configuration;
        }

        public DiscordBotBuilder UseButtonHandler(ButtonService buttonService)
        {
            _bot.Buttons = buttonService;
            _useButtonBuilder = true;
            
            return this;
        }

        public DiscordBotBuilder AddServiceProvider(IServiceProvider provider)
        {
            _bot.ServiceProvider = provider;
            return this;
        }

        public DiscordBotBuilder UseCommandHandler(int argpos, string prefix, CommandService commandService)
        {
            _useCommandHandler = true;

            _bot.ArgPos = argpos;
            _bot.StringPrefix = prefix;

            _bot.Commands = commandService;


            return this;
        }
        public DiscordBotBuilder UseModalHandler(ModalService modalservice)
        {
            _useModalHandler = true;

            _bot.Modals = modalservice;

            return this;
        }

        public DiscordBotBuilder UseMenuHandler(MenuService menuhandler)
        {
            _useMenuHandler = true;

            _bot.Menu = menuhandler;

            return this;
        }
        public DiscordBotBuilder UseLogger()
        {
            _useLogger = true;

            return this;
        }


        public DiscordBot Build()
        {
            

            _bot.ConfigureHandlers(_useButtonBuilder, _useCommandHandler, _useModalHandler, _useMenuHandler, _useLogger);

            _bot.StartAsync(_token).GetAwaiter().GetResult();

            return _bot;
        }
    }
}
