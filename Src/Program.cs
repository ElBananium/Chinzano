using Data.TradeRepository;
using Data.TradeRepositoryInRAM;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Middleware;
using Middleware.Buttons;
using Middleware.Menu;
using Middleware.Modals;
using Src;
using Src.Services.CraftingService;
using Src.Services.RepositoryLogger;
using System.Reflection;

var commands = new Discord.Commands.CommandService();
var buttonsservice = new ButtonService();
var modalservice = new ModalService();
var menuservice = new MenuService();

var configuration = new ConfigurationBuilder().AddJsonFile("botconfig.json").Build();


var repo = new RAMGenericRepository();
repo.AddNewRepository("materials", "Маты");
repo.AddNewRepository("marijusig", "Косяки");
repo.AddNewRepository("bulletproofs", "Броники");

repo.DeserializeRepo();


var bot = new DiscordBotBuilder("ODY1MjE1MzM4MjY1MzEzMjgw.Gq5KSf.2AGpn_sdEP2DwLLJECkAOUpZWNAFCWu3YfLvoc", new Discord.WebSocket.DiscordSocketConfig() { LogLevel = Discord.LogSeverity.Debug }, configuration)
    .UseCommandHandler(0, "~!~", commands)
    .UseButtonHandler(buttonsservice)
    .UseModalHandler(modalservice)
    .UseMenuHandler(menuservice)
    .UseLogger();


var provider = new ServiceCollection()
    .AddSingleton<IGenericRepository>(repo)
    .AddSingleton<ButtonService>(buttonsservice)
    .AddSingleton<IConfiguration>(configuration)
    .AddSingleton<ModalService>(modalservice)
    .AddSingleton<MenuService>(menuservice)
    .AddSingleton<DiscordSocketClient>(bot.Client)
    .AddTransient<IRepositoryLogger, RepositoryLogger>()
    .AddTransient<ICraftingService, CraftingService>()
    .BuildServiceProvider();


bot.AddServiceProvider(provider);
buttonsservice.AddServiceProvider(provider);
modalservice.AddServiceProvider(provider);
menuservice.AddServiceProvider(provider);

buttonsservice.AddModules(Assembly.GetEntryAssembly());
modalservice.AddModules(Assembly.GetEntryAssembly());
menuservice.AddModules(Assembly.GetEntryAssembly());
commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), provider);



bot.Build();

Task.Delay(-1).GetAwaiter().GetResult();