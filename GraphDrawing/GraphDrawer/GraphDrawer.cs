using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphDrawing.GraphDrawer
{
    public class GraphDrawer
    {

        private IEnumerable<SalesGraphPoint> _salesGraphPoints;

        private int _maximusDrawingCount;

        private int _divisionValue;

        private float _divisionlenght;

        private void SetMaximumDrawingCount()
        {
            var maximumincount = _salesGraphPoints.Max(x => x.Count);

            int divisionvalue = 1;


            while (maximumincount / divisionvalue >= 10)
            {
                divisionvalue = divisionvalue * 10;

            }

            _divisionValue = divisionvalue;



            _maximusDrawingCount = maximumincount + _divisionValue;
        }

        private void DrawCordLines(SKCanvas canvas)
        {
            var cordbruh = new SKPaint() { Color = SKColor.Parse("#c3c3c3"), StrokeWidth = 3 };

            var zeropoint = new SKPoint(50, 900);

            var endxpoint = new SKPoint(1000, 900);

            var endypoint = new SKPoint(50, 0);

            canvas.DrawLine(zeropoint, endypoint, cordbruh);

            canvas.DrawLine(zeropoint, endxpoint, cordbruh);
        }



        public void DrawDivisionLines(SKCanvas canvas)
        {
            var brush = new SKPaint() { Color = SKColor.Parse("#c3c3c3"), StrokeWidth = 3 };
            var textbrush = new SKPaint() { Color = SKColor.Parse("#000000"), StrokeWidth = 6, TextAlign = SKTextAlign.Center, TextSize = 14 };
            _divisionlenght = 800 / (_maximusDrawingCount / _divisionValue);

            for (int i = 1; i <= _maximusDrawingCount / _divisionValue; i++)
            {
                var p1 = new SKPoint(50, 900 - i * _divisionlenght);

                var p2 = new SKPoint(1000, 900 - i * _divisionlenght);


                canvas.DrawLine(p1, p2, brush);

                canvas.DrawText((_divisionValue * i).ToString(), new SKPoint(20, 900 - i * _divisionlenght), textbrush);
            }
        }


        public void SetPointAndNames(SKCanvas canvas)
        {
            var textbrush = new SKPaint() { Color = SKColor.Parse("#000000"), StrokeWidth = 6, TextAlign = SKTextAlign.Center, TextSize = 32 };
            int divisionlenght = 950 / (_salesGraphPoints.Count() + 1);

            var list = _salesGraphPoints.OrderBy(x => x.Day).ToArray();

            var points = new List<SKPoint>();

            for (int i = 1; i <= list.Length; i++)
            {
                canvas.DrawText(list[i - 1].Day.ToString(), new SKPoint(50 + i * divisionlenght, 940), textbrush);

                float pointpercent = (float)(list[i - 1].Count) / (_maximusDrawingCount);

                int a = (int)(900 - pointpercent * 800);

                points.Add(new SKPoint(50 + i * divisionlenght, a));
            }
            foreach (var point in points)
            {
                canvas.DrawPoint(point, textbrush);
            }

            var linebrush = new SKPaint() { Color = SKColor.Parse("#00008b"), StrokeWidth = 3 };

            var pointsarr = points.ToArray();
            for (int i = 1; i < pointsarr.Length; i++)
            {
                canvas.DrawLine(points[i - 1], points[i], linebrush);
            }

        }

        public SKBitmap GetCanvas()
        {
            var bitmap = new SKBitmap(1000, 1000);


            var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColor.Parse("#ffffff"));

            DrawCordLines(canvas);

            SetMaximumDrawingCount();

            DrawDivisionLines(canvas);

            SetPointAndNames(canvas);





            return bitmap;
        }

        public GraphDrawer(IEnumerable<SalesGraphPoint> salesGraphPoints)
        {
            _salesGraphPoints = salesGraphPoints;
        }
    }
}
