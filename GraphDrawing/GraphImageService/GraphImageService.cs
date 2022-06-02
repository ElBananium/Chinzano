using GraphDrawing.GraphDrawer;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDrawing.GraphImageService
{
    public class GraphImageService : IGraphImageService
    {

        public string GetFilePatch(IEnumerable<SalesGraphPoint> point)
        {


            var bitmap = new GraphDrawerClient(point).GetCanvas();


            SKData data = SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Png, 100);
            var random = new Random();
            if (!Directory.Exists("Graph")) Directory.CreateDirectory("Graph");
            string filename = $"Graph/file{random.Next(100000)}.png";
            Stream stream = File.Create(filename);
            
                data.SaveTo(stream);

            stream.Dispose();

            return filename;

        }
    }
}
