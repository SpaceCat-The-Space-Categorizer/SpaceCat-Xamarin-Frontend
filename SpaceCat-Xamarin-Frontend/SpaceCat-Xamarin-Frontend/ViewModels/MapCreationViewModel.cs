using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

// View Model (Map Creation page) - manages button clicks currently

namespace SpaceCat_Xamarin_Frontend
{
    class MapCreationViewModel : INotifyPropertyChanged
    {
        // ICommands allow button clicks to route to ViewModel instead of using their "Clicked" property
        ICommand tapSettings;
        ICommand tapNewArea;
        ICommand tapDeleteArea;
        ICommand tapAddArea;
        ICommand tapAddFurniture;
        ICommand tapChooseFurniture;
        public ICommand TapSettings { get { return tapSettings; } }
        public ICommand TapNewArea { get { return tapNewArea; } }
        public ICommand TapDeleteArea { get { return tapDeleteArea; } }
        public ICommand TapAddArea { get { return tapAddArea; } }
        public ICommand TapAddFurniture { get { return tapAddFurniture; } }
        public ICommand TapChooseFurniture { get { return tapChooseFurniture; } }

        public MapCreationViewModel()
        {
            // attach command functions to ICommand variables
            tapSettings = new Command(TappedSettings);
            tapNewArea = new Command(TappedNewArea);
            tapDeleteArea = new Command(TappedDeleteArea);
            tapAddArea = new Command(TappedAddArea);
            tapAddFurniture = new Command(TappedAddFurniture);
            tapChooseFurniture = new Command(TappedChooseFurniture);
        }

        private void TappedSettings(object s)
        {
            // Handles tapping the map settings button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Settings!");
        }

        private void TappedNewArea(object s)
        {
            // Handles tapping the new area button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped New Area!");
        }

        private void TappedDeleteArea(object s)
        {
            // Handles tapping the delete area button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Delete Area!");
        }

        private void TappedAddArea(object s)
        {
            // Handles tapping the add to area button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Add to Area!");
        }

        private void TappedAddFurniture(object s)
        {
            // Handles tapping the add new furniture button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Add New Furniture!");
        }

        private void TappedChooseFurniture(object s)
        {
            // Handles tapping the choose furniture button (unimplemented)
            System.Diagnostics.Debug.WriteLine("Tapped Choose Furniture!");
        }

        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
