using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.OrderSessionRepository
{
    public interface IOrderSessionRepository
    {
        public void AddNewSession(ulong userdiscordid, ulong channeldiscordid, string reponame);

        public void RemoveSession(ulong channeldiscordid);

        public OrderSession GetSession(ulong channeldiscordid);
    }
}
