using GraphDrawing.GraphDrawer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDrawing.GraphImageService
{
    public interface IGraphImageService
    {

        public string GetFilePatch(IEnumerable<SalesGraphPoint> point);
    }
}
