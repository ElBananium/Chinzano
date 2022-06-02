using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.MoneyStorage
{
    public interface IMoneyStorage
    {
        public int Count { get; }

        public void WidthDraw(uint count);

        public void Deposit(uint count);
    }
}
