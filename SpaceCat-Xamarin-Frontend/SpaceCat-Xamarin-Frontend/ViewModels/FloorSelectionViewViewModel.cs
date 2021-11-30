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
    class FloorSelectionViewViewModel
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

        public FloorSelectionViewViewModel()
        {
            Floors = new ObservableCollection<Floor>();

            // TEMP FLOORS
            //Floors.Add(new Floor(1));

            // receive updated floor from floor survey page
            MessagingCenter.Subscribe<DataEntryPage, AreaSurvey[]>(this, "SaveFloorSurvey",
                (page, surveys) =>
                {

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
            //TODO: save survey data
        }

        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
