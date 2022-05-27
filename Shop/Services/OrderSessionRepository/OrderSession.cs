using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.OrderSessionRepository
{
    public class OrderSession
    {
        public ulong UserDiscordId { get; set; }

        public OrderSessionState State { get; set; }

        public ulong ChannelId { get; set; }

        public bool IsNowDelivery = false;
        public bool IsTomorrowDelivery = false;

        public bool IsSpecialTimeDelivery = false;

        public string TradeRepoName { get; set; }

        public DateTime SpecialTime { get; set; }

        public uint HowManyNeed { get; set; }

    }
}
