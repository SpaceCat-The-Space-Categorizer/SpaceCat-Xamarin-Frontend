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

// View Model (Building List page) - manages buildings in left-hand side list view

namespace SpaceCat_Xamarin_Frontend
{
    public class BuildingListViewModel : INotifyPropertyChanged
    {
        // "Buildings" contains list of buildings displayed in list view
        //          - Observable Collections notify the UI when the list has been altered or appended
        // "SelectedBuilding" is used in the landing page XAML to update the right-hand side building info
        //          - will likely be used when moving between pages as well

        private ObservableCollection<Test_Building> _buildings;
        private Test_Building _selected;
        public ObservableCollection<Test_Building> Buildings
        {
            get { return _buildings; }
            set { _buildings = value; OnPropertyChanged(); }
        }
        public Test_Building SelectedBuilding
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged(); }
        }

        public BuildingListViewModel()
        {
            Buildings = new ObservableCollection<Test_Building>();

            // TEMPORARY - until Building class implemented, placeholder data to test UI for the building list view
            Buildings.Add(new Test_Building(1, "Building 1", "Fall 2022", "*layout*", "Ready for Survey"));
            Buildings.Add(new Test_Building(2, "Building 2", "Spring 2022", "*layout*", "Ready for Survey"));
            Buildings.Add(new Test_Building(3, "Building 3", "Spring 2021", "*layout*", "Outdated"));
            Buildings.Add(new Test_Building(4, "Building 4", "Fall 2021", "*layout*", "Outdated"));

            /* Messaging Center subscribes to recieve messages sent from other pages -
               in this case it's recieving the new or editted building object from the Test-MapCreatePage
               so it can be added to the building list on the landing page
            */
            MessagingCenter.Subscribe<Test_MapCreatePage, Test_Building>(this, "CreateBuilding", 
                (page, building) =>
                {
                    if (building.BuildingID == 0)   //if it's a newly added building
                    {
                        building.BuildingID = Buildings.Count + 1;
                        Buildings.Add(building);
                    }
                    else                            //if it's a building that has been editted
                    {
                        // the editted building's ID is matched to its uneditted version still in the Buildings collection,
                        // the old version's index is noted and that building is removed from the list,
                        // the editted version is added to the end of the list, then moved to the previously noted index
                        Test_Building oldBuilding = Buildings.Where(build => build.BuildingID == building.BuildingID).FirstOrDefault();
                        int newI = Buildings.IndexOf(oldBuilding);
                        Buildings.Remove(oldBuilding);
                        Buildings.Add(building);
                        int oldI = Buildings.IndexOf(building);
                        Buildings.Move(oldI, newI);
                        OnPropertyChanged("Buildings");
                    }
                });
        }

        public async void ImportBuilding(FileResult file)
        {
            //

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
                    // need to dispose?
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
