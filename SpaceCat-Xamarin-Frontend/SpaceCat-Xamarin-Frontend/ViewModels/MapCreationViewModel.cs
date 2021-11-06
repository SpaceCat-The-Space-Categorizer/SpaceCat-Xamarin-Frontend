using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpaceCat_Xamarin_Frontend
{
    class MapCreationViewModel : INotifyPropertyChanged
    {
        ICommand tapAreaTools;
        public ICommand TapAreaTools { get { return tapAreaTools; } }

        public MapCreationViewModel()
        {
            tapAreaTools = new Command(TappedAreaTools);
        }

        void TappedAreaTools (object s)
        {
            System.Diagnostics.Debug.WriteLine("Area Tools Tapped!!!");
        }

        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
