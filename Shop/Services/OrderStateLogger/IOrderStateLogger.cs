using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.OrderStateLogger
{
    public interface IOrderStateLogger
    {
        public Task OrderRecivedFromManager(string managername, int numberoforder, string reponame, int count);


        public Task OrderRecivedFromSystem(int numberoforder, string reponame, int count);

        public Task OrderPicked(int numberoforder, string managername);

        public Task OrderTransacted(string managername, int numberoforder, string reponame);
    }
}
