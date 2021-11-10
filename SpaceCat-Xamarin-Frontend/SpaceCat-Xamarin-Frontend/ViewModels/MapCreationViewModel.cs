﻿using System;
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
    public class MapCreationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<AreaFigure> _figures;
        private int _selectedIndex;
        private int _newFigIndex;

        /// <summary>
        ///     Contains all of the area figures currently drawn on the map.
        /// </summary>
        public ObservableCollection<AreaFigure> Figures
        {
            get { return _figures; }
            set { _figures = value; OnPropertyChanged(); }
        }
        /// <summary>
        ///     The index of the selected figure in Figures.
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; OnPropertyChanged(); }
        }
        /// <summary>
        ///     The index of the figure being drawn if one exists. Default to -1 if not.
        /// </summary>
        public int NewFigIndex
        {
            get { return _newFigIndex; }
            set { _newFigIndex = value; OnPropertyChanged(); }
        }

        public bool NewAreaToolOn;
        public bool AddAreaToolOn;
        
        string[] hexAreaColors = new string[]
            { "CCDF3E", "E06666", "F6B26B", "FFD966", "93C47D", "76A5AF",
                "6FA8DC", "8E7CC3", "C27BA0" };

        public MapCreationViewModel()
        {
            Figures = new ObservableCollection<AreaFigure>();
            SelectedIndex = -1;
            NewFigIndex = -1;

            NewAreaToolOn = false;
            AddAreaToolOn = false;

            // attach command functions to Command variables (defined below area methods)
            MapSettingsCommand = new Command(ExecuteMapSettings);
            NewAreaCommand = new Command(ExecuteNewArea);
            DeleteAreaCommand = new Command(ExecuteDeleteArea);
            AddAreaCommand = new Command(ExecuteAddArea);
            AddFurnitureCommand = new Command(ExecuteAddFurniture);
            ChooseFurnitureCommand = new Command(ExecuteChooseFurniture);

            //RunUnitTesting();   // RUN UNIT TESTS (uncomment to run, results in debug output)
        }

        /// <summary>
        ///     Handles user input on the blueprint when using area tools.
        /// </summary>
        /// <remarks>
        ///     ON TAP PRESSED <br/>
        ///         If using New or Add Area tools, this creates a new figure and adds it to Figures. <br/> <br/>
        ///         
        ///     ON TAP MOVED <br/>
        ///         When drawing a new figure, this updates the figure with the current tap location. <br/> <br/>
        ///     
        ///     ON TAP RELEASED <br/>
        ///         When drawing a new figure, this indicates the figure is complete. <br/>
        ///         When not drawing a new figure, this calls the SelectArea method for the received tap location.
        /// </remarks>
        /// <param name="tapType">Type of finger tap (pressed, moved, released).</param>
        /// <param name="tapLoc">Location of finger tap.</param>
        public void AreaInputHandler(TouchActionType tapType, Point tapLoc)
        {
            switch (tapType)
            {
                case TouchActionType.Pressed:
                    if (NewFigIndex == -1 && (NewAreaToolOn || AddAreaToolOn))
                    {
                        int randIndex = new Random().Next(hexAreaColors.Length);
                        PointCollection points = new PointCollection { tapLoc, tapLoc, tapLoc, tapLoc };
                        AreaFigure newFigure = null;
                        if (NewAreaToolOn)
                        {
                            Test_Area newArea = new Test_Area(new List<AreaFigure>(), hexAreaColors[randIndex]) ;
                            newFigure = new AreaFigure(newArea, points, 1.0);
                            
                        }
                        else if (AddAreaToolOn)
                        {
                            newFigure = new AreaFigure(Figures[SelectedIndex].Area, points, 1.0);
                        }

                        NewFigIndex = Figures.Count;
                        Figures.Add(newFigure);
                    }
                    break;
                case TouchActionType.Moved:
                    if (NewFigIndex != -1)
                    {
                        Figures[NewFigIndex].FigPoints = ChangeEndPoint(Figures[NewFigIndex].FigPoints, tapLoc);
                    }
                    break;
                case TouchActionType.Released:
                    if (NewFigIndex != -1)
                    {
                        System.Diagnostics.Debug.WriteLine("AREAS COUNT: " + Figures.Count);
                        NewFigIndex = -1;
                    }
                    else 
                        SelectArea(tapLoc);

                    NewAreaToolOn = false;
                    AddAreaToolOn = false;
                    break;
            }
        }

        /// <summary>
        ///     Selects any tapped area figure. Deselects any previously selected area figure.
        /// </summary>
        /// <remarks>
        ///     Selects only if no area tools are in use. Selected figures are set to half opacity.
        /// </remarks>
        /// <param name="tapLoc">Location of finger tap.</param>
        public void SelectArea(Point tapLoc)
        {
            if (!AddAreaToolOn && !NewAreaToolOn)
            {
                if (SelectedIndex >= 0) 
                    Figures[SelectedIndex].Opacity = 1;
                SelectedIndex = -1;

                for (int i = 0; i < Figures.Count; i++)
                {
                    if (Contains(Figures[i].FigPoints, tapLoc))
                    {
                        SelectedIndex = i;
                        Figures[i].Opacity = 0.5;
                        break;
                    }
                }
            }
        }

        /// <summary>
        ///     Changes the end point of the figure being drawn.
        /// </summary>
        /// <param name="points">Previous figure points.</param>
        /// <param name="newPoint">New desired end point.</param>
        /// <returns>Altered figure points.</returns>
        public PointCollection ChangeEndPoint(PointCollection points, Point newPoint)
        {
            return new PointCollection() { points[0], new Point(newPoint.X, points[0].Y), newPoint, new Point(points[0].X, newPoint.Y) };
        }

        /// <summary>
        ///     Determines if the provided point is contained within the bounds of the shape created by
        ///     the provided point collection.
        /// </summary>
        /// <param name="points">The points of a shape.</param>
        /// <param name="pt">The point to check if contained in shape.</param>
        /// <returns>Returns true if provided point is within the bounds of the outer points.</returns>
        public bool Contains(PointCollection points, Point pt)
        {
            if (points.Count > 0)
            {
                double lowX = points[0].X;
                double lowY = points[0].Y;
                double highX = -1;
                double highY = -1;
                foreach (Point figPt in points)
                {
                    if (figPt.X < lowX) lowX = figPt.X;
                    if (figPt.Y < lowY) lowY = figPt.Y;
                    if (figPt.X > highX) highX = figPt.X;
                    if (figPt.Y > highY) highY = figPt.Y;
                }
                if (pt.X >= lowX && pt.X <= highX && pt.Y >= lowY && pt.Y <= highY)
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

        /// <summary>
        ///     Called on click of New Area button, enables New Area tool/disables Add Area tool.
        /// </summary>
        /// <param name="s">Not Used</param>
        private void ExecuteNewArea(object s)
        {
            // Handles tapping the new area button
            AddAreaToolOn = false;
            NewAreaToolOn = true;
        }

        /// <summary>
        ///     Called on click of Add Area button, enables Add Area tool/disables New Area tool.
        /// </summary>
        /// <remarks>
        ///     Will not enable Add Area tool if an area is not selected.
        /// </remarks>
        /// <param name="s">Not Used</param>
        private void ExecuteAddArea(object s)
        {
            // Handles tapping the add to area button
            NewAreaToolOn = false;
            if (SelectedIndex > -1)
                AddAreaToolOn = true;
        }

        /// <summary>
        ///     Called on click of Delete Area button. Deletes selected figure.
        /// </summary>
        /// <remarks>
        ///     UPDATE LATER
        /// </remarks>
        /// <param name="s">Not Used</param>
        private void ExecuteDeleteArea(object s)
        {
            // Handles tapping the delete area button
            if (SelectedIndex > -1) // if an area is selected
            {
                // Edit Test_Area list, defining rectangles list or delete entire area
                if (Figures[SelectedIndex].Area.DefiningRectangles.Count < 2)
                {
                    //FUTURE: Remove area from floor
                }
                else
                {
                    for (int i = 0; i < Figures[SelectedIndex].Area.DefiningRectangles.Count; i++)
                    {
                        // delete figure from area rectangles
                        if (Figures[SelectedIndex].Area.DefiningRectangles[i] == Figures[SelectedIndex])
                            Figures[SelectedIndex].Area.DefiningRectangles.RemoveAt(i);
                    }
                }
                Figures.RemoveAt(SelectedIndex);
                SelectedIndex = -1;
            }
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


        /// <summary>
        ///     Indicates that the UI should be updated to reflect some kind of change to bound variables.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        // TESTING FUNCTIONS
        /// <summary>
        ///     Runs all method tests when called. Any failures are printed to debug output.
        /// </summary>
        private void RunUnitTesting()
        {
            // Method to call all unit tests

            Test_Contains();
        }

        /// <summary>
        ///     Tests the Contains method.
        /// </summary>
        /// <remarks>
        ///     To add tests: <br/>
        ///     Add to ptList (ptCollection index, expected result, and point to check for) <br/>
        ///     Add to ptCollections to test different point configurations.
        /// </remarks>
        private void Test_Contains()
        {
            PointCollection[] ptCollections = 
                {
                    new PointCollection { new Point(0,0), new Point(10,0), new Point(10,10), new Point(0,10) },
                    new PointCollection { new Point(0,10), new Point(0,0), new Point(10,0), new Point(10,10) },
                    new PointCollection { new Point(5,20), new Point(30,20), new Point(30,40), new Point(5,40) },
                    new PointCollection { new Point(30,20), new Point(5,40), new Point(5,20), new Point(30,40) },
                };

            List<(int, bool, Point)> ptList = new List<(int, bool, Point)>
                {
                    // TESTS 0-4
                    (0, true, new Point(5,5)),      // center of figure
                    (0, true, new Point(1,8)),      // near inner edge
                    (0, false, new Point(5,-2)),    // above figure, out of screen
                    (0, false, new Point(-2,5)),    // left of figure, out of screen
                    (0, false, new Point(-4,-4)),   // above and left of figure, out of screen on both
 
                    // TESTS 5-9
                    (1, true, new Point(5,5)),      // same as above, different point order
                    (1, true, new Point(1,8)),
                    (1, false, new Point(5,-2)),
                    (1, false, new Point(-2,5)),
                    (1, false, new Point(-4,-4)),

                    // TESTS 10-19
                    (2, true, new Point(5,20)),     // starting point
                    (2, true, new Point(30,40)),    // end point
                    (2, true, new Point(10,20)),    // top edge
                    (2, true, new Point(10,40)),    // bottom edge
                    (2, true, new Point(5,30)),     // left edge
                    (2, true, new Point(30,30)),    // right edge
                    (2, false, new Point(15,45)),   // below figure
                    (2, false, new Point(45,30)),   // right of figure
                    (2, false, new Point(15,15)),   // above figure
                    (2, false, new Point(3,30)),    // left of figure

                    // TESTS 20-29
                    (3, true, new Point(5,20)),     // same as above, different point order
                    (3, true, new Point(30,40)),
                    (3, true, new Point(10,20)),
                    (3, true, new Point(10,40)),
                    (3, true, new Point(5,30)),
                    (3, true, new Point(30,30)),
                    (3, false, new Point(15,45)),
                    (3, false, new Point(45,30)),
                    (3, false, new Point(15,15)),
                    (3, false, new Point(3,30)),
                };
            
            foreach ((int, bool, Point) test in ptList)
            {
                if (test.Item2 != Contains(ptCollections[test.Item1], test.Item3))
                    System.Diagnostics.Debug.WriteLine("Test_Contains: Test[" + ptList.IndexOf(test) + "] Failed!");
            }
            System.Diagnostics.Debug.WriteLine("Test_Contains: Complete");
        }

    }


    
}
