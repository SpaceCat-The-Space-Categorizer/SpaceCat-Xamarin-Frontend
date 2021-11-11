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
            Floor d = new Floor(4);
            Floor e = new Floor(5);
            Floor f = new Floor(6);
            Floor g = new Floor(7);
            Floor h = new Floor(8);
            Floors.Add(a);
            Floors.Add(b);
            Floors.Add(c);
            Floors.Add(d);
            Floors.Add(e);
            Floors.Add(f);
            Floors.Add(g);
            Floors.Add(h);

            if (Floors.Count > 0)
                SelectedFloor = Floors[0];
        }


        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
