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

        // SelectedFurnitureIndex is the index from SelectedFigureIndex's Area.ContainedFurniture
        public int SelectedFigureIndex;
        private int SelectedFurnitureIndex;

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
            SelectedFurnitureIndex = -1;

            SeatAddCommand = new Command(ExecuteSeatAdd);
            SeatRemoveCommand = new Command(ExecuteSeatRemove);
        }

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

        public string SelectionHandler(Point tapLoc)
        {
            for (int i = 0; i < Figures.Count; i++)
            {
                if (MapUtilities.Contains(Figures[i].FigPoints, tapLoc))
                {
                    SelectedFigureIndex = i;
                    int j = 0;
                    foreach (Furniture furn in Figures[i].Area.ContainedFurniture)
                    {
                        //if furn contains taploc, select furniture, bring up add/minus buttons
                        if (MapUtilities.ShapeContains(new FurnitureShape(furn), tapLoc))
                        {
                            SelectedFurnitureIndex = j;
                            SeatingText = "Occupied\n" + furn.OccupiedSeats + " / " + furn.Seating;
                            return "furniture";
                        }

                        j++;
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

        private void ExecuteSeatAdd(object s)
        {
            int maxSeating = Figures[SelectedFigureIndex].Area.ContainedFurniture[SelectedFurnitureIndex].Seating;
            int currentSeating = Figures[SelectedFigureIndex].Area.ContainedFurniture[SelectedFurnitureIndex].OccupiedSeats;
            if (currentSeating < maxSeating)
            {
                Figures[SelectedFigureIndex].Area.ContainedFurniture[SelectedFurnitureIndex].OccupiedSeats += 1;
                SeatingText = "Occupied\n" + (currentSeating + 1) + " / " + maxSeating;
            }

            System.Diagnostics.Debug.WriteLine(Figures[SelectedFigureIndex].Area.ContainedFurniture[SelectedFurnitureIndex].OccupiedSeats.ToString());
        }

        private void ExecuteSeatRemove(object s)
        {
            int maxSeating = Figures[SelectedFigureIndex].Area.ContainedFurniture[SelectedFurnitureIndex].Seating;
            int currentSeating = Figures[SelectedFigureIndex].Area.ContainedFurniture[SelectedFurnitureIndex].OccupiedSeats;
            if (currentSeating > 0)
            {
                Figures[SelectedFigureIndex].Area.ContainedFurniture[SelectedFurnitureIndex].OccupiedSeats -= 1;
                SeatingText = "Occupied\n" + (currentSeating - 1) + " / " + maxSeating;
            }

            System.Diagnostics.Debug.WriteLine(Figures[SelectedFigureIndex].Area.ContainedFurniture[SelectedFurnitureIndex].OccupiedSeats.ToString());
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
