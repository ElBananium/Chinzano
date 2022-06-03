using Data.MoneyStorage;
using Data.TradeRepository;
using Discord;
using Discord.WebSocket;
using Middleware;
using Middleware.Buttons;
using Middleware.Menu;
using Shop.Buttons;
using Src.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Menus
{
    public class ManagerMenu : MenuBase
    {
        private IGenericRepository _repo;
        private IMoneyStorage _moneyStorage;

        public override string PlaceHolder => null;

        public override int MinValue => 1;

        public override int MaxValue => 1;

        public override SelectMenuOptionBuilder[] GetComponent()
        {
            var allrepos = _repo.GetAllRepositories();
            var fields = new List<SelectMenuOptionBuilder>();
            fields.Add(new SelectMenuOptionBuilder("-", "notchoisen", isDefault: true));
            fields.Add(new("Бюджет", "budget"));
            foreach (var repository in allrepos)
            {
                fields.Add(new SelectMenuOptionBuilder(repository.PublicName, repository.Name));
            }
            
            return fields.ToArray();
        }

        public override async Task OnComponentExecuted(SocketMessageComponent modal)
        {
            await modal.DeferAsync();
            var info = modal.Data.Values.First();

            var repository = _repo.GetRepositoryByName(info);


            var messages = await modal.Channel.GetMessagesAsync(100).FlattenAsync();
            if (messages.Count() > 1)
            {
                await messages.First().DeleteAsync();
            }

            if (repository == null)
            {
                await AdditionalHandle(modal);
                return;
            }

            var embed = new EmbedBuilder() { Color = new Color(187, 27, 78), Title = repository.PublicName };

            embed.AddField("На складе", repository.Count.ToString());

            var components = new AdditionalComponentBuilder();
            var adinfo = new Dictionary<string, string>() { { "repname", info } };
            components.WithButton<ManagerDepositBtn>(adinfo);
            components.WithButton<ManagerWidthdrawBtn>(adinfo);
            components.WithButton<DeleteThisMsgBtn>();


            
            
            


            await modal.Channel.SendMessageAsync("", false, embed.Build(), components: components.Build());
        }


        private async Task AdditionalHandle(SocketMessageComponent arg)
        {
            if(arg.Data.Values.First() == "budget")
            {

                var components = new AdditionalComponentBuilder().WithButton<ManagerBudgetBtn>().WithButton<DeleteThisMsgBtn>();
                var embed = new EmbedBuilder() { Color = Color.Green, Title = "Бюджет : "+_moneyStorage.Count };

                await arg.Channel.SendMessageAsync(embed: embed.Build(), components: components.Build());
            }
        }

        public ManagerMenu(IGenericRepository repo, IMoneyStorage moneyStorage)
        {
            _repo = repo;
            _moneyStorage = moneyStorage;
        }
    }
}
