using GraphDrawing.GraphDrawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.GraphicsOrTableInfoHandler
{
    public interface IGraphicsOrTableInfoHandler
    {
        public IEnumerable<SalesGraphPoint> GetProfitInfo(int mounth, int from, int to);

        public IEnumerable<SalesGraphPoint> GetSalesInfo(string tradereponame, int mounth, int from, int to);

    }
}
