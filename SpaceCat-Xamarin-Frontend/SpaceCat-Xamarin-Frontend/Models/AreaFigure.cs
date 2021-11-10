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
        private Test_Area _area;    // area figure belongs to
        private PointCollection _points;
        private double _opacity;

        public Test_Area Area
        {
            get { return _area; }
            set { _area = value; OnPropertyChanged(); }
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

        public AreaFigure()
        {
            // keep to use no argument constructor
        }

        public AreaFigure(Test_Area area, PointCollection points, double opacity)
        {
            Area = area;
            FigPoints = points;
            Opacity = opacity;
        }

        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
