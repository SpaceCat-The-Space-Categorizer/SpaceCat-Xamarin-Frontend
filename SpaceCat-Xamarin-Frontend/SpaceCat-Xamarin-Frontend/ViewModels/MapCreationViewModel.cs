using SpaceCat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TouchTracking;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;


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
        
        private string[] hexAreaColors = new string[]
            { "CCDF3E", "E06666", "F6B26B", "FFD966", "93C47D", "76A5AF",
                "6FA8DC", "8E7CC3", "C27BA0" };

        private ObservableCollection<AreaFigure> _figures;
        private int _selectedIndex;
        private ImageSource _mapImage;

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

        public ImageSource MapImage
        {
            get { return _mapImage; }
            set { _mapImage = value; OnPropertyChanged(); }
        }

        public MapCreationViewModel()
        {
            NewAreaToolOn = false;
            AddAreaToolOn = false;
            FigInProgress = false;
            NewAreaList = new List<Area>();

            Figures = new ObservableCollection<AreaFigure>();
            SelectedIndex = -1;

            // attach command functions to Command variables (defined below area methods)
            MapSettingsCommand = new Command(ExecuteMapSettings);
            NewAreaCommand = new Command(ExecuteNewArea);
            DeleteAreaCommand = new Command(ExecuteDeleteArea);
            AddAreaCommand = new Command(ExecuteAddArea);
            AddFurnitureCommand = new Command(ExecuteAddFurniture);
            ChooseFurnitureCommand = new Command(ExecuteChooseFurniture);

            //MapUtilities.RunUnitTesting();   // RUN UNIT TESTS (uncomment to run, results in debug output)
        }

        /// <summary>
        ///     Loads any existing areas from the selected floor onto the map.
        /// </summary>
        /// <param name="thisFloor">The selected floor being loaded. (Or a new floor)</param>
        public void LoadFloor(Floor thisFloor)
        {
            ThisFloor = thisFloor;
            NewAreaList = thisFloor.Areas;
            foreach (Area anArea in NewAreaList)
            {
                foreach (SpaceCat.Rectangle rect in anArea.DefiningRectangles)
                {
                    Figures.Add(new AreaFigure(anArea, rect, 1.0));
                }
            }
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
        public void AreaInputHandler(TouchActionType tapType, Point tapLoc)
        {
            switch (tapType)
            {
                case TouchActionType.Pressed:
                    if (!FigInProgress && (NewAreaToolOn || AddAreaToolOn))
                    {
                        if (NewAreaToolOn)      CreateFigure(true, tapLoc);
                        else if (AddAreaToolOn) CreateFigure(false, tapLoc);
                    }
                    break;
                case TouchActionType.Moved:
                    if (FigInProgress)
                    {
                        Figures[Figures.Count - 1].ChangeEndPoint(tapLoc);
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
                    if (MapUtilities.Contains(Figures[i].FigPoints, tapLoc))
                    {
                        SelectedIndex = i;
                        Figures[i].Opacity = 0.5;
                        break;
                    }
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
        public Command AddFurnitureCommand { get; set; }
        public Command ChooseFurnitureCommand { get; set; }
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

        private async void ExecuteMapSettings(object s)
        {
            // Handles tapping the map settings button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Settings!");

            // open file picker
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Please select a floorplan file",
                    FileTypes = FilePickerFileType.Images,
                });
                if (result != null)
                {
                    MapImage = ImageSource.FromFile(result.FullPath);
                   

                }
            }
            catch (Exception e)
            {
                // exit fail
                System.Diagnostics.Debug.WriteLine("Exception on MapCreationViewModel.cs in ImportFloorplan(e2): " + e);
            }
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
