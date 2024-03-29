﻿using Data.MoneyStorage;
using Data.MoneyStorageFile;
using Data.ShopPriceFilterRepoInJson;
using Data.ShopPriceFiltersRepository;
using Data.TradeRepository;
using Data.TradeRepositoryInRAM;
using Discord;
using Discord.WebSocket;
using GraphDrawing.GraphImageService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Middleware;
using Middleware.Buttons;
using Middleware.Menu;
using Middleware.Modals;
using Shop;
using Shop.Services;
using Shop.Services.BudgenManager;
using Shop.Services.GraphicsOrTableInfoHandler;
using Shop.Services.OrderSessionRepository;
using Shop.Services.OrderStateLogger;
using Shop.Services.PlacedOrderArchive;
using Shop.Services.PlacedOrderRepository;
using Shop.Services.ShopPriceHandler;
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

var bot = new DiscordBotBuilder("ODY1MjE1MzM4MjY1MzEzMjgw.Gq5KSf.2AGpn_sdEP2DwLLJECkAOUpZWNAFCWu3YfLvoc", new Discord.WebSocket.DiscordSocketConfig() { LogLevel = LogSeverity.Debug, 
    GatewayIntents = GatewayIntents.Guilds |GatewayIntents.GuildMessages | GatewayIntents.GuildIntegrations }, configuration)
    .UseCommandHandler(0, "~!~", commands)
    .UseButtonHandler(buttonsservice)
    .UseModalHandler(modalservice)
    .UseMenuHandler(menuservice);


var ordermiddleware = new OrderShopHandler(ordersession, repo, bot.Client, new ShopPriceHandler(new ShopPriceFilterRepositoryInJson()) );

bot.Client.MessageReceived += ordermiddleware.HandleMessage;

var provider = new ServiceCollection()
    .AddSingleton<IGenericRepository>(repo)
    .AddSingleton<IConfiguration>(configuration)
    .AddSingleton<DiscordSocketClient>(bot.Client)
    .AddSingleton<IShopGenericRepository>(shoprepo)
    .AddTransient<IRepositoryLogger, RepositoryLogger>()
    .AddTransient<ICraftingService, CraftingService>()
    .AddSingleton<IOrderSessionRepository>(ordersession)
    .AddTransient<IPlacedOrderRepository, PlacedOrderRepository>()
    .AddTransient<IOrderStateLogger, OrderStateLogger>()
    .AddTransient<IShopPriceFiltersRepository, ShopPriceFilterRepositoryInJson>()
    .AddTransient<IShopPriceHandler, ShopPriceHandler>()
    .AddTransient<IMoneyStorage, MoneyStorage>()
    .AddTransient<IBudgetManager, BudgetManager>()
    .AddTransient<IPlacedOrderArchive, PlacedOrderArchive>()
    .AddTransient<IGraphicsOrTableInfoHandler, GraphicsOrTableInfoHandler>()
    .AddTransient<IGraphImageService, GraphImageService>()
    .BuildServiceProvider();

bot.AddServiceProvider(provider);

List<Assembly> assemblys = new List<Assembly>();

assemblys.Add(Assembly.GetExecutingAssembly());
assemblys.Add(typeof(IShopGenericRepository).Assembly);

foreach(var assembly in assemblys)
{
    buttonsservice.AddModules(assembly, provider, bot.Client);
    modalservice.AddModules(assembly, provider, bot.Client);
    menuservice.AddModules(assembly, provider, bot.Client);
    commands.AddModulesAsync(assembly: assembly, provider).GetAwaiter().GetResult();
}

AdditionalComponentBuilder.MenusService = menuservice;
AdditionalComponentBuilder.ButtonsService = buttonsservice;
AdditionalComponentBuilder.ModalsService = modalservice;



bot.Client.Log += Src.LogMessage.Log;

AntiExceptionHelper.client = bot.Client;
AntiExceptionHelper.config = configuration;


bot.Client.Log += AntiExceptionHelper.Log;

bot.CloseTaskEvent += AntiExceptionHelper.LogException;

bot.Build();



Task.Delay(-1).GetAwaiter().GetResult();