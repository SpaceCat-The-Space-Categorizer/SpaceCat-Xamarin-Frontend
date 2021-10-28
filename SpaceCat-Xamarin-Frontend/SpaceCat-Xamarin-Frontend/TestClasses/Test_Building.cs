using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpaceCat_Xamarin_Frontend
{
    public class Test_Building : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _date;
        private string _plan;
        private string _status;
        public int BuildingID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }
        public string BuildingName
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }
        public string BuildingDate
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(); }
        }
        public string BuildingFloorplan
        {
            get { return _plan; }
            set { _plan = value; OnPropertyChanged(); }
        }
        public string BuildingStatus
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

        public Test_Building()
        {

        }

        public Test_Building(int buildingID, string buildingName, string buildingDate, string buildingLayout, string buildingStatus)
        {
            BuildingID = buildingID;
            BuildingName = buildingName;
            BuildingDate = buildingDate;
            BuildingFloorplan = buildingLayout;
            BuildingStatus = buildingStatus;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
