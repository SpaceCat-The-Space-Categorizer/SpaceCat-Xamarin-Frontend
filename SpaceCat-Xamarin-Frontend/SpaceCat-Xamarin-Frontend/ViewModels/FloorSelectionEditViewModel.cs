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
    class FloorSelectionEditViewModel
    {
        private Building _thisBuilding;
        private ObservableCollection<Floor> _floors;
        private Floor _selected;
        public Building ThisBuilding
        {
            get { return _thisBuilding; }
            set { _thisBuilding = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Floor> Floors
        {
            get { return _floors; }
            set { _floors = value; OnPropertyChanged(); }
        }
        public Floor SelectedFloor
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged(); }
        }

        public FloorSelectionEditViewModel()
        {
            Floors = new ObservableCollection<Floor>();

            // receive updated floor from floor editing page
            MessagingCenter.Subscribe<MapCreationPage, (Floor, bool)>(this, "UpdateFloor",
                (page, floor) =>
                {
                    Floors.Add(floor.Item1);
                    if (!floor.Item2)     //if it's not a new floor
                    {
                        int ogIndex = Floors.IndexOf(SelectedFloor);
                        Floors.RemoveAt(ogIndex);
                        Floors.Move(Floors.Count - 1, ogIndex);
                    }
                    SelectedFloor = Floors[0];
                });
        }

        public void LoadBuilding(Building thisBuilding)
        {
            ThisBuilding = thisBuilding;
            if (ThisBuilding.Floors.Count > 0)
            {
                foreach (Floor f in ThisBuilding.Floors)
                    Floors.Add(f);
                SelectedFloor = Floors[0];
            }
        }

        public void SaveExit()
        {
            ThisBuilding.Floors.Clear();
            foreach (Floor f in Floors)
                ThisBuilding.AddFloor(f);
            MessagingCenter.Send(this, "UpdateBuilding", ThisBuilding);
        }

        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
