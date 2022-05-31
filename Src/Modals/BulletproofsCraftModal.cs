using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Middleware.Modals;
using Src.Services.CraftingService;
using Src.Services.RepositoryLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Modals
{
    public class BulletproofsCraftModal : ModalBase
    {
        private IGenericRepository _repos;

        private IConfiguration _config;

        private ICraftingService _craftingService;

        private IRepositoryLogger _logger;


        public override string Title => "Крафт броников";

        public override ModalComponentBuilder GetComponent()
        {
            return new ModalComponentBuilder().WithTextInput("Сколько броников скрафтить?", "count", TextInputStyle.Short);
        }

        public override async Task OnComponentExecuted(SocketModal modal)
        {
            
                int count;

                if (!int.TryParse(TextInputsValues["count"], out count)) return;

                var materialrepo = _repos.GetRepositoryByName(_config["materialsreponame"]);
                var bulletproofsrepo = _repos.GetRepositoryByName(_config["bulletproofreponame"]);

                var crafting = _craftingService;
                crafting.Configure(materialrepo, bulletproofsrepo, int.Parse(_config["pricetocraftbulletperoof"]));


                await modal.DeferAsync();
                if (!crafting.CanYouCraft(count))
                {
                var embed = new EmbedBuilder() { Title = "Ошибка | На складе недостаточно материалов", Color = Color.Red };
                await modal.User.SendMessageAsync(embed: embed.Build());
                    return;
                }


                crafting.Craft(count);
                await _logger.LogCraft(modal.User, materialrepo, bulletproofsrepo, int.Parse(_config["pricetocraftbulletperoof"]) * count, count);


        }

        public BulletproofsCraftModal(IGenericRepository repos, IConfiguration config, ICraftingService craftingService, IRepositoryLogger logger)
        {
            _repos = repos;

            _config = config;

            _craftingService = craftingService;
            _logger = logger;
        }
    }
}
