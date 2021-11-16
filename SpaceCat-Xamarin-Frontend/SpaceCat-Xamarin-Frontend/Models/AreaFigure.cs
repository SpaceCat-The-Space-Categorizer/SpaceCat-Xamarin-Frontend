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
        private PointCollection _points;
        private Point _start;
        private Point _end;
        private double _opacity;
        private Brush _fillColor;
        private Brush _strokeColor;

        public Area Area
        {
            get { return _area; }
            set { _area = value; OnPropertyChanged(); }
        }
        public PointCollection FigPoints
        {
            get { return _points; }
            set { _points = value; OnPropertyChanged(); }
        }
        public Point Start
        {
            get { return _start; }
            set { _start = value; OnPropertyChanged(); }
        }
        public Point End
        {
            get { return _end; }
            set { _end = value; OnPropertyChanged(); }
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

        public AreaFigure(Area area, Point startPoint, Point endPoint, double opacity)
        {
            Area = area;
            Start = startPoint;
            End = endPoint;
            Opacity = opacity;
            StrokeColor = new SolidColorBrush(Color.FromHex("#" + area.Color));
            FillColor = new SolidColorBrush(Color.FromHex("#33" + area.Color));
            AssignPoints();
        }

        public AreaFigure(Area area, SpaceCat.Rectangle rect, double opacity)
        {
            Area = area;
            Start = new Point(rect.TopLeft.Item1, rect.TopLeft.Item2);
            End = new Point(rect.BottomRight.Item1, rect.BottomRight.Item2);
            Opacity = opacity;
            StrokeColor = new SolidColorBrush(Color.FromHex("#" + area.Color));
            FillColor = new SolidColorBrush(Color.FromHex("#33" + area.Color));
            AssignPoints();
        }

        /// <summary>
        ///     Assign FigPoints based on Start and End point values.
        /// </summary>
        public void AssignPoints()
        {
            FigPoints = new PointCollection 
                { 
                    new Point(Start.X, Start.Y),
                    new Point(End.X, Start.Y),
                    new Point(End.X, End.Y),
                    new Point(Start.X, End.Y)
                };
        }

        /// <summary>
        ///     Changes the end point of the figure being drawn.
        /// </summary>
        /// <param name="newEndPoint">New desired end point.</param>
        public void ChangeEndPoint(Point newEndPoint)
        {
            End = newEndPoint;
            AssignPoints();
        }

        /// <summary>
        ///     Tries to create a rectangle from the AreaFigure's Start and End points.
        /// </summary>
        /// <returns>The resulting rectangle.</returns>
        public SpaceCat.Rectangle GetRectangle()
        {
            SpaceCat.Rectangle result = null;
            try { 
                result = new SpaceCat.Rectangle(Start.X, Start.Y, End.X, End.Y); }
            catch (Exception e) { 
                System.Diagnostics.Debug.WriteLine("Exception in AreaFigure at GetRectangle: " + e); }

            return result;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        ///     Indicates that the UI should be updated to reflect some kind of change to bound variables.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
