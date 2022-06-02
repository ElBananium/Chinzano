
using Discord;
using Discord.WebSocket;
using Middleware.Modals;
using Shop.Services.BudgenManager;
using System.Threading.Tasks;

namespace Shop.Modals
{
    public class ManagerWidthdrawBudgetModal : ModalBase
    {
        private IBudgetManager _budget;

        public override string Title => "Снять деньги";

        public override ModalComponentBuilder GetComponent()
        {
            return new ModalComponentBuilder()
                .WithTextInput("Сколько снять?", "storage")
                .WithTextInput("Причина", "reason");
        }

        public override async Task OnComponentExecuted(SocketModal modal)
        {
            int count;
            if (!int.TryParse(TextInputsValues["storage"], out count)) return;
            if (count <= 0) return;
            await modal.DeferAsync();
            await _budget.ManagerWidthDrawSomeMoney(Guild.GetUser(modal.User.Id).DisplayName.Split("|")[0], count, TextInputsValues["reason"]);

            
            


            var messages = await modal.Channel.GetMessagesAsync(100).FlattenAsync();
            if (messages.Count() > 1)
            {
                await messages.First().DeleteAsync();
            }


        }

        public ManagerWidthdrawBudgetModal(IBudgetManager budget)
        {
            _budget = budget;
        }
    }
}
