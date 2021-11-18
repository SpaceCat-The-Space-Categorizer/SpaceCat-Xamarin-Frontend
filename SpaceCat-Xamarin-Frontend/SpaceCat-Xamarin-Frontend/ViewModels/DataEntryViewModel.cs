﻿using SpaceCat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpaceCat_Xamarin_Frontend
{
    public class DataEntryViewModel : INotifyPropertyChanged
    {
        private Floor thisFloor;
        private ObservableCollection<AreaFigure> _figures;

        public ObservableCollection<AreaFigure> Figures
        {
            get { return _figures; }
            set { _figures = value; OnPropertyChanged(); }
        }

        public DataEntryViewModel()
        {
            Figures = new ObservableCollection<AreaFigure>();
        }

        public void LoadFloor(Floor aFloor)
        {
            thisFloor = aFloor;
            foreach(Area a in thisFloor.Areas)
            {
                foreach(SpaceCat.Rectangle rect in a.DefiningRectangles)
                {
                    Figures.Add(new AreaFigure(a, rect, 1.0));
                }
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
