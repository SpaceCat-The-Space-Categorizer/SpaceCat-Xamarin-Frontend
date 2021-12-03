using SpaceCat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace SpaceCat_Xamarin_Frontend
{
    public class DataEntryViewModel : INotifyPropertyChanged
    {
        private Floor thisFloor;

        public int SelectedFigureIndex;
        public int SelectedShapeIndex;

        private ObservableCollection<AreaFigure> _figures;
        private ObservableCollection<FurnitureShape> _shapes;
        private string _seatingText;

        public ObservableCollection<AreaFigure> Figures
        {
            get { return _figures; }
            set { _figures = value; OnPropertyChanged(); }
        }
        public ObservableCollection<FurnitureShape> Shapes
        {
            get { return _shapes; }
            set { _shapes = value; OnPropertyChanged(); }
        }
        public string SeatingText
        {
            get { return _seatingText; }
            set { _seatingText = value; OnPropertyChanged(); }
        }

        public DataEntryViewModel()
        {
            Figures = new ObservableCollection<AreaFigure>();
            Shapes = new ObservableCollection<FurnitureShape>();
            SelectedFigureIndex = -1;
            SelectedShapeIndex = -1;

            SeatAddCommand = new Command(ExecuteSeatAdd);
            SeatRemoveCommand = new Command(ExecuteSeatRemove);
        }

        /// <summary>
        /// Loads the given floor onto the screen by populating the Figures and Shapes lists.
        /// </summary>
        /// <param name="aFloor">The floor to load.</param>
        public void LoadFloor(Floor aFloor)
        {
            thisFloor = aFloor;
            foreach (Area anArea in thisFloor.Areas)
            {
                foreach (SpaceCat.Rectangle rect in anArea.DefiningRectangles)
                {
                    Figures.Add(new AreaFigure(anArea, rect, 1.0));
                }
                foreach (Furniture furn in anArea.ContainedFurniture)
                {
                    furn.ClearSurveyData();
                    Shapes.Add(new FurnitureShape(furn));
                }
            }
        }

        /// <summary>
        /// Handles any tap release interactions on the map. Updates status of furniture shapes
        /// and helps manage appearance of survey buttons.
        /// </summary>
        /// <param name="tapLoc">The location of the recorded tap.</param>
        /// <returns>Returns "furniture" if furniture was tapped, 
        /// "area" if just an area was tapped, or "none" if neither were tapped.</returns>
        public string SelectionHandler(Point tapLoc)
        {
            // check for previous selection
            if (SelectedShapeIndex != -1)
            {
                Shapes.Add(new FurnitureShape(Shapes[SelectedShapeIndex], "Counted"));
                Shapes.RemoveAt(SelectedShapeIndex);
            }

            SelectedShapeIndex = -1;
            for (int i = 0; i < Figures.Count; i++)
            {
                if (MapUtilities.Contains(Figures[i].FigPoints, tapLoc))
                {
                    SelectedFigureIndex = i;
                    foreach (Furniture furn in Figures[i].Area.ContainedFurniture)
                    {
                        //if furn contains taploc, select furniture, bring up add/minus buttons
                        for (int j = 0; j < Shapes.Count; j++)
                        {
                            if (Shapes[j].Furn == furn)
                            {
                                if (MapUtilities.ShapeContains(Shapes[j], tapLoc))
                                {
                                    Shapes.Add(new FurnitureShape(Shapes[j], "InProgress"));
                                    Shapes.RemoveAt(j);
                                    SelectedShapeIndex = Shapes.Count - 1;
                                    SeatingText = "Occupied\n" + furn.OccupiedSeats + " / " + furn.Seating;
                                    return "furniture";
                                }
                            }
                        }
                    }
                    return "area";
                }
            }
            return "none";
        }

        private void AddAreaNote(string notes)
        {
            // (unimplemented)
            Figures[SelectedFigureIndex].Area.AdditionalNotes = notes;
        }


        // USER INPUT COMMAND HANDLERS
        // Commands allow button clicks to route to ViewModel instead of using their "Clicked" property
        public Command SeatAddCommand { get; set; }
        public Command SeatRemoveCommand { get; set; }

        /// <summary>
        /// Adds 1 to the occupied seats count on the selected furniture. Updates the
        /// associated text accordingly.
        /// </summary>
        /// <param name="s">Not used here.</param>
        private void ExecuteSeatAdd(object s)
        {
            int maxSeating = Shapes[SelectedShapeIndex].Furn.Seating;
            int currentSeating = Shapes[SelectedShapeIndex].Furn.OccupiedSeats;
            if (currentSeating < maxSeating)
            {
                Shapes[SelectedShapeIndex].Furn.OccupiedSeats += 1;
                SeatingText = "Occupied\n" + (currentSeating + 1) + " / " + maxSeating;
            }
        }

        /// <summary>
        /// Subtracts 1 from the occupied seats count on the selected furniture. Updates
        /// the associated text accordingly.
        /// </summary>
        /// <param name="s">Not used here.</param>
        private void ExecuteSeatRemove(object s)
        {
            int maxSeating = Shapes[SelectedShapeIndex].Furn.Seating;
            int currentSeating = Shapes[SelectedShapeIndex].Furn.OccupiedSeats;
            if (currentSeating > 0)
            {
                Shapes[SelectedShapeIndex].Furn.OccupiedSeats -= 1;
                SeatingText = "Occupied\n" + (currentSeating - 1) + " / " + maxSeating;
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
