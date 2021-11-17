using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace SpaceCat_Xamarin_Frontend
{
    public class Test_Area : INotifyPropertyChanged
    {
        private List<AreaFigure> _rects;
        private Brush _fillColor;
        private Brush _strokeColor;

        public List<AreaFigure> DefiningRectangles
        {
            get { return _rects; }
            set { _rects = value; OnPropertyChanged(); }
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

        public Test_Area(List<AreaFigure> rects, string color)
        {
            DefiningRectangles = rects;
            StrokeColor = new SolidColorBrush(Color.FromHex("#" + color));
            FillColor = new SolidColorBrush(Color.FromHex("#33" + color));
        }

        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
