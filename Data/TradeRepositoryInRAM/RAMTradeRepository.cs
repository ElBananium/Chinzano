using Data.TradeRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.TradeRepositoryInRAM
{
    public class RAMTradeRepository : TradeRepo
    {


        public RAMTradeRepository(string publicname, string name) : base(publicname, name)
        {

        }



        public override void Deposit(long count)
        {
            lock (CountLock)
            {
                Count = Count + count;
            }
            
        }

        public override void SetPricePerIten(uint price)
        {
            lock (CountLock)
            {
                PricePerItem = price;
            }
        }

        public override void ToTrade(long count)
        {
            if (count > Count) throw new InvalidDataException("Вы хотите переместить в резерв больше чем есть");

            lock (CountLock)
            {
                lock (ToTradeCountLock)
                {
                    Count = Count - count;

                    ToTradeCount = ToTradeCount + count;
                }

                
            }
            
        }

        public override void Traded(long count)
        {
            if(count > ToTradeCount) throw new InvalidDataException("Вы хотите убрать из резерва больше чем есть");

            lock (ToTradeCountLock)
            {
                ToTradeCount = ToTradeCount - count;
            }
            
        }

        public override void Withdraw(long count)
        {
            if (count > Count) throw new InvalidDataException("Вы хотите снять больше чем есть");
            lock (CountLock)
            {
                Count = Count - count;
            }
            
        }


    }
}
