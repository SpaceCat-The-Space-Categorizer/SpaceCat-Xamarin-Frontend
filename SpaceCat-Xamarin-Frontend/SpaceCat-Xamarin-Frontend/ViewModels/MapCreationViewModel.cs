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
        // Areas            - contains all completed area polygons on map
        // SelectedID       - contains ID of selected area polygon (if none are selected is set to -1)
        // FigureInProgress - contains area polygon currently being created by user (added to Areas on tap release)

        public List<Polygon> Areas;
        public int SelectedID;
        public Polygon FigureInProgress;
        
        public bool NewAreaToolOn;
        public bool AddAreaToolOn;
        public bool DeleteAreaToolOn;
        
        string[] hexAreaColors = new string[]
            { "CCDF3E", "E06666", "F6B26B", "FFD966", "93C47D", "76A5AF",
                "6FA8DC", "8E7CC3", "C27BA0" };

        public MapCreationViewModel()
        {
            NewAreaToolOn = false;
            AddAreaToolOn = false;
            DeleteAreaToolOn = false;

            //Areas = currentFloor.Areas;
            Areas = new List<Polygon>();
            SelectedID = -1;

            // attach command functions to Command variables (defined below area methods)
            MapSettingsCommand = new Command(ExecuteMapSettings);
            NewAreaCommand = new Command(ExecuteNewArea);
            DeleteAreaCommand = new Command(ExecuteDeleteArea);
            AddAreaCommand = new Command(ExecuteAddArea);
            AddFurnitureCommand = new Command(ExecuteAddFurniture);
            ChooseFurnitureCommand = new Command(ExecuteChooseFurniture);
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
                strokeColor = new SolidColorBrush(Color.FromHex("#" + hexAreaColors[0]));
                fillColor = new SolidColorBrush(Color.FromHex("#33" + hexAreaColors[0]));
            }
            Polygon newAreaShape = new Polygon
            {
                Points = points,
                Fill = fillColor,
                Stroke = strokeColor,
                StrokeThickness = 5
            };
            

            FigureInProgress = newAreaShape;
            return newAreaShape;
        }

        public void AreaCreationHandler(TouchActionEventArgs args)
        {
            switch(args.Type)
            {
                case TouchActionType.Moved:
                    if (FigureInProgress != null)
                    {
                        FigureInProgress.Points = ChangeEndPoint(FigureInProgress.Points, new Point(args.Location.X, args.Location.Y));
                    }
                    break;
                case TouchActionType.Released:  //mouse up
                    if (FigureInProgress != null)
                    {
                        if (NewAreaToolOn)
                            Areas.Add(FigureInProgress);
                        else if (AddAreaToolOn) 
                            Areas.Add(FigureInProgress); //TODO: change to add to selected area when implementing add area tool
                        FigureInProgress = null;
                    }
                    else
                    {
                        if (DeleteAreaToolOn)
                        {
                            //TODO: delete area
                            System.Diagnostics.Debug.WriteLine("Sorry! I can't delete rectangles yet... good try tho :)");
                        }
                    }
                    NewAreaToolOn = false;
                    AddAreaToolOn = false;
                    DeleteAreaToolOn = false;
                    break;
            }
        }


        public void SelectArea(Point tapLoc)
        {
            if (!AddAreaToolOn && !DeleteAreaToolOn)
            {
                if (SelectedID >= 0) Areas[SelectedID].Opacity = 1;
                SelectedID = -1;
                for (int i = 0; i < Areas.Count; i++)
                {
                    if (Contains(Areas[i], tapLoc))
                    {
                        SelectedID = i;
                        Areas[i].Opacity = 0.5;
                        break;
                    }
                }
            }
        }

        public PointCollection ChangeEndPoint(PointCollection points, Point newPoint)
        {
            return new PointCollection() { points[0], new Point(newPoint.X, points[0].Y), newPoint, new Point(points[0].X, newPoint.Y) };
        }

        public bool Contains(Polygon figure, Point pt)
        {
            if (figure.Points.Count > 0)
            {
                double lowX = figure.Points[0].X;
                double lowY = figure.Points[0].Y;
                double highX = -1;
                double highY = -1;
                foreach (Point p in figure.Points)
                {
                    if (p.X < lowX) lowX = p.X;
                    if (p.Y < lowY) lowY = p.Y;
                    if (p.X > highX) highX = p.X;
                    if (p.Y > highY) highY = p.Y;
                }
                if (pt.X > lowX && pt.X < highX && pt.Y > lowY && pt.Y < highX)
                    return true;
                else return false;
            }
            else return false;
        }


        // USER INPUT COMMAND HANDLERS
        // Commands allow button clicks to route to ViewModel instead of using their "Clicked" property
        public Command MapSettingsCommand { get; set; }
        public Command NewAreaCommand { get; set; }
        public Command DeleteAreaCommand { get; set; }
        public Command AddAreaCommand { get; set; }
        public Command AddFurnitureCommand { get; set; }
        public Command ChooseFurnitureCommand { get; set; }

        private void ExecuteMapSettings(object s)
        {
            // Handles tapping the map settings button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Settings!");
        }

        private void ExecuteNewArea(object s)
        {
            // Handles tapping the new area button
            NewAreaToolOn = true;
        }

        private void ExecuteDeleteArea(object s)
        {
            // Handles tapping the delete area button
            DeleteAreaToolOn = true;
        }

        private void ExecuteAddArea(object s)
        {
            // Handles tapping the add to area button
            AddAreaToolOn = true;
        }

        private void ExecuteAddFurniture(object s)
        {
            // Handles tapping the add new furniture button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Add New Furniture!");
        }

        private void ExecuteChooseFurniture(object s)
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
