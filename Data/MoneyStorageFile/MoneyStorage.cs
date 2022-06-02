using Data.MoneyStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MoneyStorageFile
{
    public class MoneyStorage : IMoneyStorage
    {
        public static object MoneyLock = new object();

        private int moneycount;

        private object moneycountlock = new object();

        public int Count => moneycount;

        public void Deposit(uint count)
        {
            lock (moneycountlock)
            {
                moneycount += (int)count;
            }
            UpdateValue();
        }

        public void WidthDraw(uint count)
        {

            lock (moneycountlock)
            {
                if (moneycount >= count)
                {
                    moneycount = moneycount - (int)count;
                    UpdateValue();
                }
                else
                {
                    throw new Exception("Не хватает денег в бюджете");
                }


            }
            
        }

        private void UpdateValue()
        {
            lock (MoneyLock)
            {
                using(StreamWriter sw = new("money.json"))
                {
                    sw.Write(moneycount);
                }
            }
        }

        public MoneyStorage()
        {
            if (!File.Exists("money.json"))
            {
                var money = File.Create("money.json");
                money.Dispose();
                
            }

            lock (MoneyLock)
            {
                string count;
                using(StreamReader sr = new("money.json"))
                {
                    count = sr.ReadToEnd();
                }
                lock (moneycountlock)
                {

                    if (!int.TryParse(count, out moneycount)) moneycount = 0;
                }
            }


        }
    }
}
