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
        public Building ThisBuilding;

        private ObservableCollection<Floor> _floors;
        private Floor _selected;
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

        private bool NewFloorAdded;

        public FloorSelectionEditViewModel()
        {
            Floors = new ObservableCollection<Floor>();
            NewFloorAdded = false;

            // receive updated floor from floor editing page
            MessagingCenter.Subscribe<MapCreationPage, Floor>(this, "UpdateFloor",
                (page, floor) =>
                {
                    Floors.Add(floor);
                    if (!NewFloorAdded)
                    {
                        int ogIndex = Floors.IndexOf(SelectedFloor);
                        Floors.RemoveAt(ogIndex);
                        Floors.Move(Floors.Count - 1, ogIndex);
                    }
                    SelectedFloor = Floors[0];
                    NewFloorAdded = false;
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

        public Floor ExecuteNewFloor()
        {
            Floor newFloor = new Floor(Floors.Count + 1);
            NewFloorAdded = true;
            return newFloor;
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
