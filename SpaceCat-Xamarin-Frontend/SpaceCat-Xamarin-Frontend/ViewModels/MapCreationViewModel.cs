using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TouchTracking;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

// View Model (Map Creation page) - manages button clicks currently

namespace SpaceCat_Xamarin_Frontend
{
    class MapCreationViewModel : INotifyPropertyChanged
    {
        public bool newAreaToolOn;
        public bool addAreaToolOn;

        public Polygon inProgressArea;
        public Point startLocation;
        public Point endLocation;
        
        // ICommands allow button clicks to route to ViewModel instead of using their "Clicked" property
        ICommand tapSettings;
        ICommand tapNewArea;
        ICommand tapDeleteArea;
        ICommand tapAddArea;
        ICommand tapAddFurniture;
        ICommand tapChooseFurniture;
        ICommand drawArea;
        public ICommand TapSettings { get { return tapSettings; } }
        public ICommand TapNewArea { get { return tapNewArea; } }
        public ICommand TapDeleteArea { get { return tapDeleteArea; } }
        public ICommand TapAddArea { get { return tapAddArea; } }
        public ICommand TapAddFurniture { get { return tapAddFurniture; } }
        public ICommand TapChooseFurniture { get { return tapChooseFurniture; } }
        public ICommand DrawArea { get { return drawArea; } }

        string[] hexAreaColors = new string[]
            { "CCDF3E", "E06666", "F6B26B", "FFD966", "93C47D", "76A5AF",
                "6FA8DC", "8E7CC3", "C27BA0" };

        public MapCreationViewModel()
        {
            newAreaToolOn = false;
            addAreaToolOn = false;
            startLocation = new Point(0, 0);
            endLocation = new Point(0, 0);

            // attach command functions to ICommand variables
            tapSettings = new Command(TappedSettings);
            tapNewArea = new Command(TappedNewArea);
            tapDeleteArea = new Command(TappedDeleteArea);
            tapAddArea = new Command(TappedAddArea);
            tapAddFurniture = new Command(TappedAddFurniture);
            tapChooseFurniture = new Command(TappedChooseFurniture);
            drawArea = new Command(DrawingArea);
        }

        public void AreaCreationHandler(TouchActionEventArgs args)
        {
            switch(args.Type)
            {
                case TouchActionType.Moved:
                    if (inProgressArea != null)
                    {
                        UpdateArea(new Point(args.Location.X, args.Location.Y));
                    }
                    break;
                case TouchActionType.Released:
                    //mouse up
                    if (inProgressArea != null)
                    {
                        if (newAreaToolOn)
                        {
                            // TODO: create new area
                            inProgressArea = null;
                            newAreaToolOn = false;
                        }
                        else if (addAreaToolOn)
                        {
                            // TODO: add to selected area
                            addAreaToolOn = false;
                        }
                    }
                    break;
            }
        }

            public Polygon CreateArea(PointCollection points)
        {
            // returns a new polygon object from the provided points
            // color of polygon is currently randomized from hexAreaColors array

            int randIndex = new Random().Next(hexAreaColors.Length);
            SolidColorBrush strokeColor = new SolidColorBrush(Color.FromHex("#" + hexAreaColors[randIndex]));
            // first string is alpha channel, should be same for every area
            SolidColorBrush fillColor = new SolidColorBrush(Color.FromHex("#33" + hexAreaColors[randIndex])); 
            Polygon newArea = new Polygon
            {
                Points = points,
                Fill = fillColor,
                Stroke = strokeColor,
                StrokeThickness = 5
            };
            startLocation = new Point(points[0].X, points[0].Y);
            endLocation = new Point(points[2].X, points[2].Y);
            inProgressArea = newArea;
            return newArea;
        }

        public void UpdateArea(Point endPoint)
        {
            endLocation = endPoint;
            inProgressArea.Points = new PointCollection { startLocation, new Point(endLocation.X, startLocation.Y), endLocation, new Point(startLocation.X, endLocation.Y) };
        }

        public void DrawingArea(object s)
        {
            
        }

        private void TappedSettings(object s)
        {
            // Handles tapping the map settings button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Settings!");
        }

        private void TappedNewArea(object s)
        {
            // Handles tapping the new area button
            System.Diagnostics.Debug.WriteLine("Tapped New Area!");
            newAreaToolOn = true;
        }

        private void TappedDeleteArea(object s)
        {
            // Handles tapping the delete area button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Delete Area!");
        }

        private void TappedAddArea(object s)
        {
            // Handles tapping the add to area button
            System.Diagnostics.Debug.WriteLine("Tapped Add to Area!");
            addAreaToolOn = true;
        }

        private void TappedAddFurniture(object s)
        {
            // Handles tapping the add new furniture button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Add New Furniture!");
        }

        private void TappedChooseFurniture(object s)
        {
            // Handles tapping the choose furniture button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Choose Furniture!");
        }

        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
