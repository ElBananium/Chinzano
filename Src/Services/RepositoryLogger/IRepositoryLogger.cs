using Data.TradeRepository;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Services.RepositoryLogger
{
    public interface IRepositoryLogger
    {
        public Task LogDeposit(SocketUser user, TradeRepo repo, long count);

        public Task LogWidthDraw(SocketUser user, TradeRepo repo, long count);

        public Task LogCraft(SocketUser user, TradeRepo fromrepo, TradeRepo toRepo, long howmanyused, long howmanycrafted);
    }
}
