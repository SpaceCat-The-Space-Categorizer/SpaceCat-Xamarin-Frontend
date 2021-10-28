using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpaceCat_Xamarin_Frontend
{
    public class BuildingListViewModel : INotifyPropertyChanged
    {
        // BUILDING LIST VARIABLES
        private ObservableCollection<Test_Building> _buildings;
        private Test_Building _selectedBuilding;
        public ObservableCollection<Test_Building> Buildings
        {
            get { return _buildings; }
            set { _buildings = value; OnPropertyChanged(); }
        }
        public Test_Building SelectedBuilding
        {
            get { return _selectedBuilding; }
            set { _selectedBuilding = value; OnPropertyChanged(); }
        }

        public BuildingListViewModel()
        {
            Buildings = new ObservableCollection<Test_Building>();

            // TEMPORARY - need Building class, placeholder data to display List View
            Buildings.Add(new Test_Building(1, "Building 1", "Fall 2022", "*layout*", "Ready for Survey"));
            Buildings.Add(new Test_Building(2, "Building 2", "Spring 2022", "*layout*", "Ready for Survey"));
            Buildings.Add(new Test_Building(3, "Building 3", "Spring 2021", "*layout*", "Outdated"));
            Buildings.Add(new Test_Building(4, "Building 4", "Fall 2021", "*layout*", "Outdated"));

            MessagingCenter.Subscribe<Test_MapCreatePage, Test_Building>(this, "CreateBuilding", 
                (page, building) =>
                {
                    if (building.BuildingID == 0)
                    {
                        building.BuildingID = Buildings.Count + 1;
                        Buildings.Add(building);
                    }
                    else
                    {
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
