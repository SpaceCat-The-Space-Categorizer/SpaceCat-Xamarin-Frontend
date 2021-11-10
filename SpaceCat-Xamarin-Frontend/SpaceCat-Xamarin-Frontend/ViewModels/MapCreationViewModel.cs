using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private List<Polygon> _areas;
        private int _selectedID;
        private Polygon _figInProgress;

        public List<Polygon> Areas
        {
            get { return _areas; }
            set { _areas = value; OnPropertyChanged(); }
        }
        public int SelectedID
        {
            get { return _selectedID; }
            set { _selectedID = value; OnPropertyChanged(); }
        }
        public Polygon FigInProgress
        {
            get { return _figInProgress; }
            set { _figInProgress = value; OnPropertyChanged(); }
        }

        public bool NewAreaToolOn;
        public bool AddAreaToolOn;
        public bool DeleteAreaToolOn;
        
        string[] hexAreaColors = new string[]
            { "CCDF3E", "E06666", "F6B26B", "FFD966", "93C47D", "76A5AF",
                "6FA8DC", "8E7CC3", "C27BA0" };

        public MapCreationViewModel()
        {
            Areas = new List<Polygon>();
            SelectedID = -1;
            
            NewAreaToolOn = false;
            AddAreaToolOn = false;
            DeleteAreaToolOn = false;

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

            FigInProgress = newAreaShape;
            return newAreaShape;
        }

        public List<Polygon> AreaCreationHandler(TouchActionType tapType, Point tapLoc)
        {
            // INPUT: TAP MOVED
            // if a figure is in progress: changes point collection as finger moves
            // INPUT: TAP RELEASED
            // if a figure is in progress:
            //      creating new area: adds figure in progress to Areas list
            //      adding to area: (currently just adds to area list) adds figure to selected area's rect list
            // if no figure is in progress:
            //      deleting area: (currently not implemented) removes figure from Area list and map


            switch (tapType)
            {
                case TouchActionType.Pressed:
                    //mouse down
                    if (NewAreaToolOn || AddAreaToolOn)
                    {
                        PointCollection points = new PointCollection { tapLoc, tapLoc, tapLoc, tapLoc };
                        CreateArea(points);
                    }
                    break;
                case TouchActionType.Moved:
                    if (FigInProgress != null)
                    {
                        FigInProgress.Points = ChangeEndPoint(FigInProgress.Points, tapLoc);
                    }
                    break;
                case TouchActionType.Released:
                    if (FigInProgress != null)
                    {
                        if (NewAreaToolOn)
                        {
                            Areas.Add(FigInProgress);
                        }
                        else if (AddAreaToolOn)
                            Areas.Add(FigInProgress);
                        FigInProgress = null; //TODO: change to add to selected area when implementing add area tool
                    }
                    else
                    {
                        SelectArea(tapLoc);
                        if (DeleteAreaToolOn)
                        {
                            if (SelectedID >= 0) //an area is selected
                            {
                                Areas.RemoveAt(SelectedID);
                                SelectedID = -1;
                            }
                            DeleteAreaToolOn = false;
                        }
                    }
                    NewAreaToolOn = false;
                    AddAreaToolOn = false;
                    DeleteAreaToolOn = false;
                    break;
            }
            List<Polygon> allAreas = Areas;
            if (FigInProgress != null)
                allAreas.Add(FigInProgress);
            return allAreas;
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
