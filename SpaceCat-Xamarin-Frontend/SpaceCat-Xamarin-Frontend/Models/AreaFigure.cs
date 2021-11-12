using SpaceCat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace SpaceCat_Xamarin_Frontend
{
    public class AreaFigure : INotifyPropertyChanged
    {
        private Area _area;    // area figure belongs to
        private SpaceCat.Rectangle _rect;
        private PointCollection _points;
        private double _opacity;
        private Brush _fillColor;
        private Brush _strokeColor;

        public Area Area
        {
            get { return _area; }
            set { _area = value; OnPropertyChanged(); }
        }
        public SpaceCat.Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; OnPropertyChanged(); }
        }
        public PointCollection FigPoints
        {
            get { return _points; }
            set { _points = value; OnPropertyChanged(); }
        }
        
        public double Opacity
        {
            get { return _opacity; }
            set { _opacity = value; OnPropertyChanged(); }
        }
        public Brush FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; OnPropertyChanged(); }
        }
        public Brush StrokeColor
        {
            get { return _strokeColor; }
            set { _strokeColor = value; OnPropertyChanged(); }
        }

        public AreaFigure()
        {
            // keep to use no argument constructor
        }

        public AreaFigure(Area area, PointCollection points, double opacity)
        {
            Area = area;
            Rect = null;
            FigPoints = points;
            Opacity = opacity;
            StrokeColor = new SolidColorBrush(Color.FromHex("#" + area.Color));
            FillColor = new SolidColorBrush(Color.FromHex("#33" + area.Color));
        }

        public void AddRectangle(SpaceCat.Rectangle rect)
        {
            Rect = rect;
        }

        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
