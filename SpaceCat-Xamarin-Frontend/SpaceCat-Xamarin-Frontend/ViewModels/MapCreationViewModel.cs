//using SpaceCat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TouchTracking;
using TouchTracking.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

// View Model (Map Creation page) - manages button clicks currently

namespace SpaceCat_Xamarin_Frontend
{
    class MapCreationViewModel : INotifyPropertyChanged
    {
        //public Building currentBuilding;
        //public Floor currentFloor;
        //public List<Area> Areas;
        public List<Polygon> Areas;
        
        public int SelectedRectID;
        public Polygon InProgressRect;
        public Point StartLocation;
        public Point EndLocation;
        
        public bool NewAreaToolOn;
        public bool AddAreaToolOn;
        public bool DeleteAreaToolOn;
        
        
        // ICommands allow button clicks to route to ViewModel instead of using their "Clicked" property
        ICommand tapSettings;
        ICommand tapNewArea;
        ICommand tapDeleteArea;
        ICommand tapAddArea;
        ICommand tapAddFurniture;
        ICommand tapChooseFurniture;
        public ICommand TapSettings { get { return tapSettings; } }
        public ICommand TapNewArea { get { return tapNewArea; } }
        public ICommand TapDeleteArea { get { return tapDeleteArea; } }
        public ICommand TapAddArea { get { return tapAddArea; } }
        public ICommand TapAddFurniture { get { return tapAddFurniture; } }
        public ICommand TapChooseFurniture { get { return tapChooseFurniture; } }

        string[] hexAreaColors = new string[]
            { "CCDF3E", "E06666", "F6B26B", "FFD966", "93C47D", "76A5AF",
                "6FA8DC", "8E7CC3", "C27BA0" };

        public MapCreationViewModel()
        {
            NewAreaToolOn = false;
            AddAreaToolOn = false;
            DeleteAreaToolOn = false;
            StartLocation = new Point(0, 0);
            EndLocation = new Point(0, 0);

            //Areas = currentFloor.Areas;
            Areas = new List<Polygon>();
            SelectedRectID = -1;

            // attach command functions to ICommand variables
            tapSettings = new Command(TappedSettings);
            tapNewArea = new Command(TappedNewArea);
            tapDeleteArea = new Command(TappedDeleteArea);
            tapAddArea = new Command(TappedAddArea);
            tapAddFurniture = new Command(TappedAddFurniture);
            tapChooseFurniture = new Command(TappedChooseFurniture);
        }

        public Polygon CreateArea(PointCollection points)
        {
            // returns a new polygon object from the provided points
            // color of polygon is currently randomized from hexAreaColors array

            Brush strokeColor = new SolidColorBrush();
            Brush fillColor = new SolidColorBrush();
            if (NewAreaToolOn)
            {
                int randIndex = new Random().Next(hexAreaColors.Length);
                strokeColor = new SolidColorBrush(Color.FromHex("#" + hexAreaColors[randIndex]));
                // first string is alpha channel, should be same for every area
                fillColor = new SolidColorBrush(Color.FromHex("#33" + hexAreaColors[randIndex]));
            }
            else if (AddAreaToolOn)
            {
                strokeColor = new SolidColorBrush(Color.FromHex("#" + hexAreaColors[0])); //SelectedArea.DefiningRectangles[0].Stroke;
                fillColor = new SolidColorBrush(Color.FromHex("#33" + hexAreaColors[0])); //SelectedArea.Fill;
            }
            Polygon newAreaShape = new Polygon
            {
                Points = points,
                Fill = fillColor,
                Stroke = strokeColor,
                StrokeThickness = 5
            };
            // attach toucheffect
            TouchEffect touchEffect = new TouchEffect
            {
                Capture = true
            };
            touchEffect.TouchAction += OnTouchEffectAction;
            newAreaShape.Effects.Add(touchEffect);

            StartLocation = new Point(points[0].X, points[0].Y);
            EndLocation = new Point(points[2].X, points[2].Y);
            InProgressRect = newAreaShape;
            return newAreaShape;
        }

        public void AreaCreationHandler(TouchActionEventArgs args)
        {
            switch(args.Type)
            {
                case TouchActionType.Moved:
                    if (InProgressRect != null)
                    {
                        UpdateArea(new Point(args.Location.X, args.Location.Y));
                    }
                    break;
                case TouchActionType.Released:  //mouse up
                    if (InProgressRect != null)
                    {
                        if (NewAreaToolOn)
                        {
                            /*Area newArea = new Area(); // change to type area
                            newArea.AddAreaRectangle(ConvertPolygon(inProgressRect)); */
                            //TODO: don't add if polygon is too small
                            Areas.Add(InProgressRect);
                        }
                        else if (AddAreaToolOn)
                        {
                            // check if area has been selected
                            if (SelectedRectID >= 0)
                            {
                                //Areas[SelectedAreaIndex].AddAreaRectangle(ConvertPolygon(inProgressRect));
                                System.Diagnostics.Debug.WriteLine("I SELECTED A RECTANGLE WOOOOO!");
                            }
                        }
                        InProgressRect = null;
                    }
                    else
                    {
                        if (DeleteAreaToolOn)
                        {
                            //TODO: delete area
                            System.Diagnostics.Debug.WriteLine("Sorry! I can't delete rectangles yet... good try tho :)");
                        }
                    }
                    break;
            }
        }

        public void UpdateArea(Point endPoint)
        {
            // updates in progress area shape with current mouse location endpoint


            EndLocation = endPoint;
            InProgressRect.Points = new PointCollection { StartLocation, new Point(EndLocation.X, StartLocation.Y), EndLocation, new Point(StartLocation.X, EndLocation.Y) };
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            if ((AddAreaToolOn || DeleteAreaToolOn) && args.Type == TouchActionType.Released)
            {
                //SelectedRectID = obj.StyleId;
                
                foreach(Polygon shape in Areas)
                {
                    Xamarin.Forms.Rectangle r = new Xamarin.Forms.Rectangle(shape.Points[0].X, shape.Points[0].Y, shape.Points[2].X - shape.Points[0].X, shape.Points[2].Y - shape.Points[0].Y);
                    if (r.Contains(new Point(args.Location.X, args.Location.Y)))
                    {
                        SelectedRectID = Areas.IndexOf(shape);
                        System.Diagnostics.Debug.WriteLine("HALLO I SELECTED AN ITEM!!!");
                    }
                }
            }
        }

        /*
        private SpaceCat.Rectangle ConvertPolygon(Polygon rect)
        {
            // converts polygon to rectangle
            SpaceCat.Rectangle result = new SpaceCat.Rectangle((float)rect.Points[0].X, (float)rect.Points[0].Y, (float)rect.Points[2].X, (float)rect.Points[2].Y);
            return result;
        }
        */


        // USER INPUT COMMAND HANDLERS

        private void TappedSettings(object s)
        {
            // Handles tapping the map settings button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Settings!");
        }

        private void TappedNewArea(object s)
        {
            // Handles tapping the new area button
            if (!NewAreaToolOn) NewAreaToolOn = true;
            else NewAreaToolOn = false;
        }

        private void TappedDeleteArea(object s)
        {
            // Handles tapping the delete area button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Delete Area!");
            if (!DeleteAreaToolOn) DeleteAreaToolOn = true;
            else DeleteAreaToolOn = false;
        }

        private void TappedAddArea(object s)
        {
            // Handles tapping the add to area button
            if (!AddAreaToolOn) AddAreaToolOn = true;
            else AddAreaToolOn = false;
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
