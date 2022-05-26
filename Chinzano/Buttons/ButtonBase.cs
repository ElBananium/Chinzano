using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
namespace Middleware.Buttons
{
    public abstract class ButtonBase
    {
        public string CustomId { get
            {
                return "ChinzanoBotButton_" + this.GetType().Name +"_"+ JsonConvert.SerializeObject(AdditionalInfo);
            } }

        public ButtonBase()
        {
            AdditionalInfo = new Dictionary<string, string>();
        }

        public abstract ButtonBuilder GetButton();

        public abstract Task OnButtonClicked(SocketMessageComponent arg, Dictionary<string, string> info);

        public Dictionary<string, string> AdditionalInfo { get; set; }

    }
}
