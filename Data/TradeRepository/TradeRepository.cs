using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.TradeRepository
{
    public abstract class TradeRepo
    {
        protected readonly object CountLock = new object();

        protected readonly object ToTradeCountLock = new object();


        public long Count { get; protected set; }

        public long ToTradeCount { get; protected set; }

        public readonly string PublicName;

        public readonly string Name;
        public abstract void Deposit(long count);

        public abstract void Withdraw (long count);

        public abstract void ToTrade(long count);

        public abstract void Traded(long count);

        public TradeRepo(string publicname, string name)
        {
            PublicName = publicname;
            Name = name;
            Count = 0;
            ToTradeCount = 0;
        }

    }
}
