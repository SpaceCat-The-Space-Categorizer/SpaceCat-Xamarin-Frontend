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
        ICommand tapSaveExit;
        public ICommand TapSaveExit { get { return tapSaveExit; } }

        public MapCreationViewModel()
        {
            tapSaveExit = new Command(TappedSaveExit);
        }

        void TappedSaveExit (object s)
        {
            
        }

        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
