using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDrawing.GraphDrawer
{
    public class SalesGraphPoint
    {

        public int Day { get; set; }


        public int Count { get; set; }


        public SalesGraphPoint(int day, int count)
        {
            Day = day;

            Count = count;
        }
    }
}
