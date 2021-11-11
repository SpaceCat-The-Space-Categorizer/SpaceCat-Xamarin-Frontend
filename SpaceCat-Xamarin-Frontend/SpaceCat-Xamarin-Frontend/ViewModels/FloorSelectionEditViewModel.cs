using SpaceCat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpaceCat_Xamarin_Frontend
{
    class FloorSelectionEditViewModel
    {
        public ObservableCollection<Floor> _floors;
        public Floor _selected;

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

            Floor a = new Floor(1);
            Floor b = new Floor(2);
            Floor c = new Floor(3);
            Floors.Add(a);
            Floors.Add(b);
            Floors.Add(c);
        }


        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
