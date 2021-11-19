using SpaceCat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TouchTracking;
using Xamarin.Forms;

// View Model (Map Creation page) - manages button clicks currently

namespace SpaceCat_Xamarin_Frontend
{
    public class MapCreationViewModel : INotifyPropertyChanged
    {
        private Floor ThisFloor;
        private bool NewAreaToolOn;
        private bool AddAreaToolOn;
        private bool FigInProgress;
        private List<Area> NewAreaList;
        private List<FurnitureBlueprint> Templates;
        
        private string[] hexAreaColors = new string[]
            { "CCDF3E", "E06666", "F6B26B", "FFD966", "93C47D", "76A5AF",
                "6FA8DC", "8E7CC3", "C27BA0" };

        private ObservableCollection<AreaFigure> _figures;
        private int _selectedIndex;

        private ObservableCollection<FurnitureShape> _shapes;
        private int _selectedShape;

        public Grid Presets;

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

        public ObservableCollection<FurnitureShape> Shapes
        {
            get { return _shapes; }
            set { _shapes = value; OnPropertyChanged(); }
        }
        public int SelectedShape
        {
            get { return _selectedShape; }
            set { _selectedShape = value; OnPropertyChanged(); }
        }

        public MapCreationViewModel()
        {
            NewAreaToolOn = false;
            AddAreaToolOn = false;
            FigInProgress = false;
            NewAreaList = new List<Area>();
            Templates = new List<FurnitureBlueprint>();

            Figures = new ObservableCollection<AreaFigure>();
            SelectedIndex = -1;

            Shapes = new ObservableCollection<FurnitureShape>();
            SelectedShape = -1;

            // attach command functions to Command variables (defined below area methods)
            MapSettingsCommand = new Command(ExecuteMapSettings);
            NewAreaCommand = new Command(ExecuteNewArea);
            DeleteAreaCommand = new Command(ExecuteDeleteArea);
            AddAreaCommand = new Command(ExecuteAddArea);

            //MapUtilities.RunUnitTesting();   // RUN UNIT TESTS (uncomment to run, results in debug output)
        }

        /// <summary>
        ///     Loads any existing areas from the selected floor onto the map.
        /// </summary>
        /// <param name="thisFloor">The selected floor being loaded. (Or a new floor)</param>
        public ImageButton[] LoadFloor(Floor thisFloor)
        {
            ThisFloor = thisFloor;
            NewAreaList = thisFloor.Areas;
            foreach (Area anArea in NewAreaList)
            {
                foreach (SpaceCat.Rectangle rect in anArea.DefiningRectangles)
                {
                    Figures.Add(new AreaFigure(anArea, rect, 1.0));
                }
                foreach (Furniture furn in anArea.ContainedFurniture)
                {
                    //Shapes.Add();
                }
            }

            Templates.Add(new FurnitureBlueprint("table1.png", 4));
            Templates.Add(new FurnitureBlueprint("table1v2.png", 4));
            Templates.Add(new FurnitureBlueprint("table2.png", 6));
            Templates.Add(new FurnitureBlueprint("table3.png", 4));
            Templates.Add(new FurnitureBlueprint("table4.png", 2));
            Templates.Add(new FurnitureBlueprint("table4v2.png", 2));
            Templates.Add(new FurnitureBlueprint("table5.png", 2));
            Templates.Add(new FurnitureBlueprint("table5v2.png", 2));
            Templates.Add(new FurnitureBlueprint("table6.png", 1));
            Templates.Add(new FurnitureBlueprint("table6v2.png", 1));
            Templates.Add(new FurnitureBlueprint("table6v3.png", 1));
            Templates.Add(new FurnitureBlueprint("table6v4.png", 1));
            Templates.Add(new FurnitureBlueprint("table7.png", 6));
            Templates.Add(new FurnitureBlueprint("table7v2.png", 6));

            ImageButton[] buttons = new ImageButton[Templates.Count];
            for (int i = 0; i < Templates.Count; i++)
            {
                buttons[i] = FurniturePreset(Templates[i].Filepath);
            }

            //define num presets
            return buttons;
            
            
        }

        /// <summary>
        ///     Creates a new AreaFigure when adding an area rectangle using area tools.
        /// </summary>
        /// <param name="isNewArea">Is true when the figure doesn't belong to the same area as an existing AreaFigure.</param>
        /// <param name="origin">The tap location where the user started drawing the figure.</param>
        private void CreateFigure(bool isNewArea, Point origin)
        {
            Area thisArea;

            if (isNewArea)
            {
                int randIndex = new Random().Next(hexAreaColors.Length);
                thisArea = new Area(hexAreaColors[randIndex]);
                NewAreaList.Add(thisArea);
            }
            else
            {
                thisArea = Figures[SelectedIndex].Area;
            }

            // create area figure and add to figures
            AreaFigure newFigure = new AreaFigure(thisArea, origin, origin, 1.0);
            FigInProgress = true;
            Figures.Add(newFigure);
        }

        /// <summary>
        ///     Handles user input on the blueprint when using area tools.
        /// </summary>
        /// <remarks>
        ///     Needs to be updated for adding furniture tools.
        /// </remarks>
        /// <param name="tapType">Type of finger tap (pressed, moved, released).</param>
        /// <param name="tapLoc">Location of finger tap.</param>
        public int MapInputHandler(TouchActionType tapType, Point tapLoc)
        {
            int movedIndex = -1;
            switch (tapType)
            {
                case TouchActionType.Pressed:
                    if (!FigInProgress && (NewAreaToolOn || AddAreaToolOn))
                    {
                        if (NewAreaToolOn)      CreateFigure(true, tapLoc);
                        else if (AddAreaToolOn) CreateFigure(false, tapLoc);
                    }
                    else
                    {
                        for (int i = 0; i < Shapes.Count; i++)
                        {
                            if (MapUtilities.ShapeContains(Shapes[i], tapLoc))
                            {
                                SelectedShape = i;
                                break;
                            }
                        }
                    }
                    break;
                case TouchActionType.Moved:
                    if (FigInProgress)
                    {
                        Figures[Figures.Count - 1].ChangeEndPoint(tapLoc);
                    }
                    else if (SelectedShape != -1)
                    {
                        Shapes[SelectedShape].Move(tapLoc);
                        movedIndex = SelectedShape;
                    }    
                    break;
                case TouchActionType.Released:
                    if (FigInProgress)
                    {
                        // last figure added to figures is finalized in figures and new area list
                        Figures[Figures.Count - 1].ChangeEndPoint(tapLoc);
                        int affectedArea = -1;
                        for (int i = 0; i < NewAreaList.Count; i++)
                        {
                            if (NewAreaList[i] == Figures[Figures.Count - 1].Area)
                            {
                                affectedArea = i;
                                break;
                            }
                        }
                        NewAreaList[affectedArea].AddAreaRectangle(Figures[Figures.Count - 1].GetRectangle());

                        FigInProgress = false;
                    }
                    else if (SelectedShape != -1)
                    {
                        Shapes[SelectedShape].Move(tapLoc);
                        SelectedShape = -1;
                    }
                    else 
                        SelectArea(tapLoc);

                    NewAreaToolOn = false;
                    AddAreaToolOn = false;
                    break;
            }
            return movedIndex;
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
                    if (MapUtilities.Contains(Figures[i].FigPoints, tapLoc))
                    {
                        SelectedIndex = i;
                        Figures[i].Opacity = 0.5;
                        break;
                    }
                }
            }
        }

        public ImageButton FurniturePreset(string fileName)
        {
            return new ImageButton
            {
                Source = fileName,
                BackgroundColor = Color.White,
                BorderColor = Color.Black,
                BorderWidth = 1,
                CornerRadius = 5,
                HeightRequest = 60,
                CommandParameter = fileName
            };
        }

        public void AddNewFurniture(ImageButton imgButton)
        {
            // Handles tapping the add new furniture button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Add New Furniture!");

            foreach (FurnitureBlueprint blueprint in Templates)
            {
                if (blueprint.Filepath == (string)imgButton.CommandParameter)
                {
                    Shapes.Add(new FurnitureShape(blueprint));
                    break;
                }
            }


        }

        /// <summary>
        ///     Updates the current floor's list of areas with any changes made during floor editing.
        /// </summary>
        /// <returns>The current floor with new and updated areas.</returns>
        public Floor UpdateFloor()
        {
            ThisFloor.Areas = NewAreaList;
            return ThisFloor;
        }


        // USER INPUT COMMAND HANDLERS
        // Commands allow button clicks to route to ViewModel instead of using their "Clicked" property
        public Command NewAreaCommand { get; set; }
        public Command AddAreaCommand { get; set; }
        public Command DeleteAreaCommand { get; set; }
        public Command MapSettingsCommand { get; set; }

        /// <summary>
        ///     Called on click of New Area button, enables New Area tool/disables Add Area tool.
        /// </summary>
        /// <param name="s">Not Used</param>
        private void ExecuteNewArea(object s)
        {
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
            if (SelectedIndex > -1) // if an area is selected
            {
                // look for affected area in NewAreaList
                int affectedArea = -1;
                for (int i = 0; i < NewAreaList.Count; i++)
                {
                    if (NewAreaList[i] == Figures[SelectedIndex].Area)
                    {
                        affectedArea = i;
                        break;
                    }
                }
                
                // determine if it is the last rectangle in the area
                if (NewAreaList[affectedArea].DefiningRectangles.Count <= 1)
                    NewAreaList.RemoveAt(affectedArea);
                else
                    NewAreaList[affectedArea].RemoveRectangle(Figures[SelectedIndex].GetRectangle());

                Figures.RemoveAt(SelectedIndex);
                SelectedIndex = -1;
            }
        }

        private void ExecuteMapSettings(object s)
        {
            // Handles tapping the map settings button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Settings!");
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
