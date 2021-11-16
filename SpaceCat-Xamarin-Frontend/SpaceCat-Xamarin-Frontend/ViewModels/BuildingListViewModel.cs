using SpaceCat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

// View Model (Building List page) - manages buildings in left-hand side list view, handles importing new buildings

namespace SpaceCat_Xamarin_Frontend
{
    public class BuildingListViewModel : INotifyPropertyChanged
    {
        // "Buildings" contains list of buildings displayed in list view
        //          - Observable Collections notify the UI when the list has been altered or appended
        // "SelectedBuilding" is used in the landing page XAML to update the right-hand side building info
        //          - will likely be used when moving between pages as well

        private ObservableCollection<BuildingListItem> _buildings;
        private BuildingListItem _selected;
        public ObservableCollection<BuildingListItem> Buildings
        {
            get { return _buildings; }
            set { _buildings = value; OnPropertyChanged(); }
        }
        public BuildingListItem SelectedBuilding
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged(); }
        }

        public BuildingListViewModel()
        {
            Buildings = new ObservableCollection<BuildingListItem>();

            // TEMPORARY - placeholder data to test UI for the building list view
            Buildings.Add(new BuildingListItem(new Building("Building 1")));
            Buildings.Add(new BuildingListItem(new Building("Building 2")));
            Buildings.Add(new BuildingListItem(new Building("Building 3")));
            Buildings.Add(new BuildingListItem(new Building("Building 4")));

            /* Messaging Center subscribes to recieve messages sent from other pages -
               in this case it's recieving the new or editted building object from the Test-MapCreatePage
               so it can be added to the building list on the landing page */

            MessagingCenter.Subscribe<FloorSelectionEditViewModel, (Building, bool)>(this, "UpdateBuilding",
                (page, building) =>
                {
                    //Add building
                    if (building.Item2)
                    {
                        Buildings.Add(new BuildingListItem(building.Item1));
                        Buildings.Move(Buildings.Count - 1, 0);
                        OnPropertyChanged("Buildings");
                    }
                    else //update building
                    {
                        BuildingListItem oldBuilding = Buildings.FirstOrDefault(build => build.Build.Name == building.Item1.Name);
                        Buildings.Remove(oldBuilding);
                        Buildings.Add(new BuildingListItem(building.Item1));
                        Buildings.Move(Buildings.Count - 1, 0);
                        OnPropertyChanged("Buildings");
                    }
                });
        }

        public async void ImportBuilding(FileResult file)
        {
            // attempts to open and read from provided file, currently prints results to debug output
            // TODO: parse building json file, make sure provided file contains expected data
            //          add to building list and default select it

            try
            {
                using (var stream = await file.OpenReadAsync())
                {
                    byte[] b = new byte[1024];
                    UTF8Encoding encode = new UTF8Encoding(true);
                    while (stream.Read(b, 0, b.Length) > 0)
                    {
                        System.Diagnostics.Debug.WriteLine(encode.GetString(b));
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception on BuildingListViewModel.cs in ImportBuilding: " + e.Message);
            }
        }


        // INotifyPropertyChanged interface is used to update the UI when variables are altered
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
