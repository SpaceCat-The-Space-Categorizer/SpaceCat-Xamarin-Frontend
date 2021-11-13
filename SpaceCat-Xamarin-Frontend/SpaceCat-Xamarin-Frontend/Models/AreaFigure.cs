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
            //ensure Rectangle's start and end points aren't the same
            if (startPoint.X == endPoint.X || startPoint.Y == endPoint.Y)
                Rect = new SpaceCat.Rectangle(startPoint.X, startPoint.Y, endPoint.X + 1, endPoint.Y + 1);
            else
                Rect = new SpaceCat.Rectangle(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
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
            Rect = rect;
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
            if (Start.X == newEndPoint.X)
                newEndPoint.X += 0.1;
            if (Start.Y == newEndPoint.Y)
                newEndPoint.Y += 0.1;
            Rect = new SpaceCat.Rectangle(Start.X, Start.Y, newEndPoint.X, newEndPoint.Y);

            End = newEndPoint;
            AssignPoints();
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
