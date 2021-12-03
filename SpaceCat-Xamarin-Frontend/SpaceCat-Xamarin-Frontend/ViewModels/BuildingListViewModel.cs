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
        private List<RecentBuilding> RecentBuildings;

        private ObservableCollection<BuildingListItem> _buildings;
        private BuildingListItem _selected;

        /// <summary>
        /// Contains list of BuildingListItems representing buildings displayed in the list view on the landing page.
        /// </summary>
        /// <remarks>Observable Collections notify the UI when the list has been altered or appended.</remarks>
        public ObservableCollection<BuildingListItem> Buildings
        {
            get { return _buildings; }
            set { _buildings = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Used in the landing page XAML to update the right-hand side containing the selected building info.
        /// </summary>
        public BuildingListItem SelectedBuilding
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged(); }
        }

        public BuildingListViewModel()
        {
            Buildings = new ObservableCollection<BuildingListItem>();
            Persistence.ValidateEnvironment();
            RecentBuildings = Persistence.LoadRecentBuildings();
            foreach (RecentBuilding build in RecentBuildings)
            {
                Buildings.Add(new BuildingListItem(build));
            }
            
            //LoadBuildings();
            if (Buildings.Count > 0)
                SelectedBuilding = Buildings[0];

            /* Messaging Center subscribes to recieve messages sent from other pages -
               in this case it's recieving the new or editted building object from the FloorSelectionEditPage
               so it can be added to the building list on the landing page */

            MessagingCenter.Subscribe<FloorSelectionEditViewModel, RecentBuilding>(this, "UpdateBuilding",
                (page, building) =>
                {
                    bool newBuild = true;
                    for (int i = 0; i < Buildings.Count; i++)
                    {
                        if (Buildings[i].Build.Name == building.Name)
                        {
                            newBuild = false;
                            Buildings.Remove(Buildings[i]);
                            Buildings.Add(new BuildingListItem(building));
                            Buildings.Move(Buildings.Count - 1, 0);
                            break;
                        }
                    }
                    // if no matching name was found
                    if (newBuild)
                    {
                        Buildings.Add(new BuildingListItem(building));
                        Buildings.Move(Buildings.Count - 1, 0);
                    }

                    RecentBuildings.Clear();
                    foreach (BuildingListItem item in Buildings)
                    {
                        RecentBuildings.Add(item.Build);
                    }
                    Persistence.SaveRecentBuildings(RecentBuildings);
                });
        }

        /// <summary>
        /// UPDATE LATER: Currently adds temporary placeholder data to Buildings to test UI
        /// </summary>
        public void LoadBuildings()
        {
            /*Buildings.Add(new BuildingListItem(new Building("Building 1")));
            Buildings.Add(new BuildingListItem(new Building("Building 2")));
            Buildings.Add(new BuildingListItem(new Building("Building 3")));
            Buildings.Add(new BuildingListItem(new Building("Building 4")));
            Building b1 = Persistence.LoadBuilding("Building 1");
            Building b2 = Persistence.LoadBuilding("Building 2");
            Building b3 = Persistence.LoadBuilding("Building 3");
            Building b4 = Persistence.LoadBuilding("Building 4");
            Buildings.Add(new BuildingListItem(b1));
            Buildings.Add(new BuildingListItem(b2));
            Buildings.Add(new BuildingListItem(b3));
            Buildings.Add(new BuildingListItem(b4));*/


            System.Diagnostics.Debug.WriteLine("HERE I AM");
            System.Diagnostics.Debug.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            if (Buildings.Count > 0)
                SelectedBuilding = Buildings[0];
        }

        /// <summary>
        /// Attempts to open and read the provided file.
        /// </summary>
        /// <param name="file">The file chosed in the file picker.</param>
        public async void ImportBuilding(FileResult file)
        {
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
