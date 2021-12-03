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
    class FloorSelectionViewViewModel : INotifyPropertyChanged
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

            MessagingCenter.Subscribe<DataEntryPage, Floor>(this, "FloorSurvey",
                (page, floor) =>
                {
                    for (int i = 0; i < ThisBuilding.Floors.Count; i++)
                    {
                        if (ThisBuilding.Floors[i].FloorName == floor.FloorName)
                        {
                            ThisBuilding.Floors[i] = floor;
                            break;
                        }
                    }
                });
        }

        public void LoadBuilding(Building thisBuilding)
        {
            ThisBuilding = thisBuilding;
            if (ThisBuilding.Floors.Count > 0) // TODO: check if floors list exists first
            {
                foreach (Floor f in ThisBuilding.Floors)
                    Floors.Add(f);
                SelectedFloor = Floors[0];
            }
        }

        public void SaveExit()
        {
            ThisBuilding.CompleteSurvey();
        }

        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
