using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using Xamarin.Forms;

namespace SpaceCat_Xamarin_Frontend
{
    class AreaFigure
    {
        private SKPoint _p1, _p2;
        public SKPoint StartPoint { set { _p1 = value; MakeRectangle(); } }
        public SKPoint EndPoint { set { _p2 = value; MakeRectangle(); } }
        public SKColor Color { get; set; }
        public SKRect Rectangle { get; set; }
        public Point LastTouchLocation { get; set; }
        public AreaFigure()
        {

        }

        private void MakeRectangle()
        {
            Rectangle = new SKRect(_p1.X, _p1.Y, _p2.X, _p2.Y).Standardized;
        }

        public bool IsContained(SKPoint pt)
        {
            if (pt.X > _p1.X && pt.X < _p2.X && pt.Y > _p1.Y && pt.Y < _p2.Y)
                return true;
            else return false;
        }
    }
}
