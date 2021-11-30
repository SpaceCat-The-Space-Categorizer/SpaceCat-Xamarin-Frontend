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
        private ObservableCollection<AreaFigure> _figures;
        private ObservableCollection<FurnitureShape> _shapes;

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

        // SelectedFurnitureIndex is the index from SelectedFigureIndex's Area.ContainedFurniture
        public int SelectedFigureIndex;
        private int SelectedFurnitureIndex;

        public DataEntryViewModel()
        {
            Figures = new ObservableCollection<AreaFigure>();
            Shapes = new ObservableCollection<FurnitureShape>();
            SelectedFigureIndex = -1;
            SelectedFurnitureIndex = -1;
        }

        public void LoadFloor(Floor aFloor)
        {
            thisFloor = aFloor;
            foreach(Area anArea in thisFloor.Areas)
            {
                foreach(SpaceCat.Rectangle rect in anArea.DefiningRectangles)
                {
                    Figures.Add(new AreaFigure(anArea, rect, 1.0));
                }
                foreach(Furniture furn in anArea.ContainedFurniture)
                {
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
                            return "furniture";
                        }

                        j++;
                    }

                    return "area";
                }
            }
            return "none";
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
