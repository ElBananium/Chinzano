using Data.TradeRepository;
using Data.TradeRepositoryInRAM;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Middleware;
using Middleware.Buttons;
using Middleware.Menu;
using Middleware.Modals;
using Shop;
using Shop.Services;
using Shop.Services.OrderSessionRepository;
using Src;
using Src.Services.CraftingService;
using Src.Services.RepositoryLogger;
using System.Reflection;

var commands = new Discord.Commands.CommandService();
var buttonsservice = new ButtonService();
var modalservice = new ModalService();
var menuservice = new MenuService();

var configuration = new ConfigurationBuilder().AddJsonFile("botconfig.json").Build();


var repo = new RAMGenericRepository("repositories.json");
repo.AddNewRepository("materials", "Маты");
repo.AddNewRepository("marijusig", "Косяки");
repo.AddNewRepository("bulletproofs", "Броники");

repo.DeserializeRepo();

var shoprepo = new ShopGenericRepository();

shoprepo.AddExistRepository(repo.GetRepositoryByName("marijusig"));
shoprepo.AddExistRepository(repo.GetRepositoryByName("bulletproofs"));

var ordersession = new OrderSessionRepository();

var bot = new DiscordBotBuilder("ODY1MjE1MzM4MjY1MzEzMjgw.Gq5KSf.2AGpn_sdEP2DwLLJECkAOUpZWNAFCWu3YfLvoc", new Discord.WebSocket.DiscordSocketConfig() { LogLevel = Discord.LogSeverity.Debug }, configuration)
    .UseCommandHandler(0, "~!~", commands)
    .UseButtonHandler(buttonsservice)
    .UseModalHandler(modalservice)
    .UseMenuHandler(menuservice)
    .UseLogger();


var ordermiddleware = new OrderShopHandler(ordersession, menuservice, repo, bot.Client, buttonsservice);

bot.Client.MessageReceived += ordermiddleware.HandleMessage;

var provider = new ServiceCollection()
    .AddSingleton<IGenericRepository>(repo)
    .AddSingleton<ButtonService>(buttonsservice)
    .AddSingleton<IConfiguration>(configuration)
    .AddSingleton<ModalService>(modalservice)
    .AddSingleton<MenuService>(menuservice)
    .AddSingleton<DiscordSocketClient>(bot.Client)
    .AddSingleton<IShopGenericRepository>(shoprepo)
    .AddTransient<IRepositoryLogger, RepositoryLogger>()
    .AddTransient<ICraftingService, CraftingService>()
    .AddSingleton<IOrderSessionRepository>(ordersession)

    .BuildServiceProvider();


bot.AddServiceProvider(provider);
buttonsservice.AddServiceProvider(provider);
modalservice.AddServiceProvider(provider);
menuservice.AddServiceProvider(provider);


List<Assembly> assemblys = new List<Assembly>();

assemblys.Add(Assembly.GetExecutingAssembly());
assemblys.Add(typeof(IShopGenericRepository).Assembly);

foreach(var assembly in assemblys)
{
    buttonsservice.AddModules(assembly);
    modalservice.AddModules(assembly);
    menuservice.AddModules(assembly);
    commands.AddModulesAsync(assembly: assembly, provider).GetAwaiter().GetResult();
}





bot.Build();

Task.Delay(-1).GetAwaiter().GetResult();