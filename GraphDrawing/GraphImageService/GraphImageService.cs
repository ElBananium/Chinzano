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
            var bitmap = new SKBitmap(1000, 1000);


            var canvas = new SKCanvas(bitmap);

            canvas.Clear(SKColor.Parse("#ffffff"));

            var p1 = new SKPoint(0, 0);

            var p2 = new SKPoint(500, 500);

            var paint = new SKPaint() { Color = SKColor.Parse("#000000"), StrokeWidth = 10 };
            canvas.DrawLine(p1, p2, paint);


            SKData data = SKImage.FromBitmap(bitmap).Encode(SKEncodedImageFormat.Png, 100);
            var random = new Random();
            string filename = $"Graph/file{random.Next(100000)}.png";
            Stream stream = File.Create(filename);
            
                data.SaveTo(stream);

            stream.Dispose();

            return filename;

        }
    }
}
