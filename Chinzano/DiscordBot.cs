﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Middleware.Buttons;
using Middleware.Menu;
using Middleware.Modals;

namespace Middleware
{
    public class DiscordBot
    {
        public DiscordSocketClient Client { get; set; }




        public int ArgPos;

        public string StringPrefix { get; set; }
        public CommandService Commands { get; set; }

        public ButtonService Buttons { get; set; }

        public ModalService Modals { get; set; }

        public MenuService Menu { get; set; }

        public IConfiguration Configuration;

        public IServiceProvider ServiceProvider;

        public DiscordBot(DiscordSocketConfig botconfig)
        {
            Client = new DiscordSocketClient(botconfig);

        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null) return;


            if (!message.HasStringPrefix(StringPrefix, ref ArgPos) || message.Author.IsBot) return;

            var context = new SocketCommandContext(Client, message);

           await Commands.ExecuteAsync(
                context
                , argPos: ArgPos
                , services: ServiceProvider);
                

        }

        private async Task HandleButtonAsync(SocketMessageComponent arg)
        {
            await Buttons.ExecuteComponentAsync(arg);
        }


        public void ConfigureHandlers(bool UseButtonHandler, bool UseCommandHandler, bool UseModalHandler, bool UseMenuHandler, bool UseLogger)
        {

            if (UseButtonHandler) Client.ButtonExecuted += HandleButtonAsync;
            if (UseCommandHandler) Client.MessageReceived += HandleCommandAsync;
            if (UseModalHandler) Client.ModalSubmitted += HandleModalAsync;
            if (UseMenuHandler) Client.SelectMenuExecuted += HandleMenuAsync;
            if (UseLogger) Client.Log += Log;
        }

        private Task Log(LogMessage arg)
        {
            Console.Write(DateTime.Now.ToString() + " | ");
            Console.Write("[");
            switch (arg.Severity)
            {
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write("Debug");
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Verbose");
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("Info");
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Warning");
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("Error");
                    break;
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Critical");
                    break;


            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("] | ");
            Console.WriteLine(arg.Message);
            if(arg.Exception != null)
            {
                Console.WriteLine(arg.Exception.Message);
                Console.WriteLine(arg.Exception.StackTrace);
                Console.WriteLine(arg.Exception.InnerException);
            }
            return Task.CompletedTask;
        }

        private async Task HandleMenuAsync(SocketMessageComponent arg)
        {
            await Menu.ExecuteComponentAsync(arg);
        }

        private async Task HandleModalAsync(SocketModal arg)
        {
            await Modals.ExecuteComponentAsync(arg);
        }

        public async Task StartAsync(string token)
        {

            await Client.LoginAsync(Discord.TokenType.Bot, token);


            await Client.StartAsync();
        }


        
    }
}
