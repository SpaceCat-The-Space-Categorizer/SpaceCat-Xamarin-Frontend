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

        //public Command NewFloorCommand { get; set; }

        public FloorSelectionEditViewModel()
        {
            Floors = new ObservableCollection<Floor>();
            
            if (Floors.Count > 0)
                SelectedFloor = Floors[0];

            MessagingCenter.Subscribe<MapCreationPage, Floor>(this, "UpdateFloor",
                (page, floor) =>
                {
                    if (ThisBuilding.Floors.Count > 0)
                    {
                        for (int i = 0; i < ThisBuilding.Floors.Count; i++)
                        {
                            if (ThisBuilding.Floors[i] == SelectedFloor)
                            {
                                ThisBuilding.Floors[i] = floor;
                                break;
                            }
                        }
                    }
                    else
                        ThisBuilding.AddFloor(floor);
                });

            //NewFloorCommand = new Command(ExecuteNewFloor);
        }

        public Floor ExecuteNewFloor()
        {
            Floor newFloor = new Floor(Floors.Count);
            Floors.Add(newFloor);
            return newFloor;
        }

        public void SaveExit()
        {
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
