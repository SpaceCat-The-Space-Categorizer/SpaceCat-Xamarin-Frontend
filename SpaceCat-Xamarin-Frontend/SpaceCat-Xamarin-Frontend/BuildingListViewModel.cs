using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpaceCat_Xamarin_Frontend
{
    public class BuildingListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand GoToSurveyMode => new Command(Clicked_Survey);

        // SELECTED BUILDING VARIABLES
        private string _name;
        private string _date;
        private string _floorplan;
        private string _status;
        public string Name 
        { get { return _name; }
            set 
            {
                _name = value;
                OnPropertyChanged();
            } }
        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }
        public string FloorPlan
        {
            get { return _floorplan; }
            set
            {
                _floorplan = value;
                OnPropertyChanged();
            }
        }
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        // BUILDING LIST VARIABLE
        public ObservableCollection<string> Buildings { get; set; }

        public BuildingListViewModel()
        {
            Buildings = new ObservableCollection<string>();

            Buildings.Add("Building 1");
            Buildings.Add("Building 2");
            Buildings.Add("Building 3");
            Buildings.Add("Building 4");
        }

        private void Clicked_Survey()
        {
            // ALTER ON IMPLEMENT - need Building class, placeholder data to check binding
            this.Name = "Building Name";
            this.Date = "March 2022";
            this.FloorPlan = "*floorplan image*";
            this.Status = "Status: Ready for Survey";

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
