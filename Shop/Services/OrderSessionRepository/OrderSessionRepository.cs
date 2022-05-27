using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.OrderSessionRepository
{
    public class OrderSessionRepository : IOrderSessionRepository
    {
        private Dictionary<ulong, OrderSession> _orders;

        public void AddNewSession(ulong userdiscordid, ulong channeldiscordid, string reponame)
        {
            var session = new OrderSession() { UserDiscordId = userdiscordid, ChannelId = channeldiscordid, State = OrderSessionState.HowManyNeed, TradeRepoName = reponame };

            _orders.Add(channeldiscordid, session);
        }

        public OrderSession GetSession(ulong channeldiscordid)
        {
            if (!_orders.TryGetValue(channeldiscordid, out var session)) return null;
            return session;
        }

        public void RemoveSession(ulong channeldiscordid)
        {
            _orders.Remove(channeldiscordid);
        }

        public OrderSessionRepository()
        {
            _orders= new Dictionary<ulong, OrderSession>();
        }
    }
}
